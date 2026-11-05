using FcgPaymentsApi.Models;

namespace FcgPaymentsApi.Services;

public class PaymentProcessor
{
    private readonly ILogger<PaymentProcessor> _logger;
    private readonly Random _random = new();

    public PaymentProcessor(ILogger<PaymentProcessor> logger)
    {
        _logger = logger;
    }

    public async Task<PaymentProcessedEvent> ProcessPayment(OrderPlacedEvent orderEvent)
    {
        _logger.LogInformation($"Processando pagamento para OrderId: {orderEvent.OrderId}");

        // Simular processamento do pagamento (delay de 1-3 segundos)
        await Task.Delay(_random.Next(1000, 3000));

        // Simular aprovação/rejeição (90% de aprovação)
        var isApproved = _random.Next(100) < 90;

        var paymentEvent = new PaymentProcessedEvent
        {
            OrderId = orderEvent.OrderId,
            UserId = orderEvent.UserId,
            GameId = orderEvent.GameId,
            Price = orderEvent.Price,
            Status = isApproved ? "Approved" : "Rejected",
            Reason = isApproved ? null : "Saldo insuficiente",
            Timestamp = DateTime.UtcNow
        };

        _logger.LogInformation($"Pagamento {paymentEvent.Status} para OrderId: {orderEvent.OrderId}");

        return paymentEvent;
    }
}
