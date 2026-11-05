using System.Text.Json;
using FcgPaymentsApi.Data;
using FcgPaymentsApi.Models;

namespace FcgPaymentsApi.Services;

public class OrderEventConsumer : BackgroundService
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly PaymentProcessor _paymentProcessor;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrderEventConsumer> _logger;

    public OrderEventConsumer(
        IRabbitMqService rabbitMqService,
        PaymentProcessor paymentProcessor,
        IServiceProvider serviceProvider,
        ILogger<OrderEventConsumer> logger)
    {
        _rabbitMqService = rabbitMqService;
        _paymentProcessor = paymentProcessor;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _rabbitMqService.StartConsuming("order.placed", async (message) =>
        {
            try
            {
                var orderEvent = JsonSerializer.Deserialize<OrderPlacedEvent>(message);
                if (orderEvent != null)
                {
                    _logger.LogInformation($"Pedido recebido: OrderId={orderEvent.OrderId}, UserId={orderEvent.UserId}, GameId={orderEvent.GameId}, Price={orderEvent.Price}");

                    // Processar pagamento
                    var paymentResult = await _paymentProcessor.ProcessPayment(orderEvent);

                    // Salvar no banco
                    await SavePayment(orderEvent, paymentResult.Status);

                    // Publicar resultado
                    _rabbitMqService.PublishMessage("payment.processed", paymentResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao processar pedido: {ex.Message}");
            }
        });

        return Task.CompletedTask;
    }

    private async Task SavePayment(OrderPlacedEvent orderEvent, string status)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = orderEvent.OrderId,
            UserId = orderEvent.UserId,
            GameId = orderEvent.GameId,
            Amount = orderEvent.Price,
            Status = status,
            ProcessedAt = DateTime.UtcNow
        };

        dbContext.Payments.Add(payment);
        await dbContext.SaveChangesAsync();
    }
}
