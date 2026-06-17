using DepositoryOrderProcessor.Domain.Orders;

namespace DepositoryOrderProcessor.Application.Orders;

public sealed record OrderProcessingResult(
    Guid OrderId,
    OrderStatus Status,
    decimal? AvailableQuantity,
    string? RejectionReason
);