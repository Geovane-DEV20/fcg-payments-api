namespace FcgPaymentsApi.Models;

public class OrderPlacedEvent
{
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
    public decimal Price { get; set; }
    public Guid OrderId { get; set; }
    public DateTime Timestamp { get; set; }
}

public class PaymentProcessedEvent
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; } = string.Empty; // "Approved" or "Rejected"
    public DateTime Timestamp { get; set; }
    public string? Reason { get; set; }
}

public class Payment
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
}
