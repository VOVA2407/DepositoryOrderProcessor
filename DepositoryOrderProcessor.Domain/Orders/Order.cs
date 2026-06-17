using DepositoryOrderProcessor.Domain.ValueObjects;

namespace DepositoryOrderProcessor.Domain.Orders;

public sealed class Order
{
    public Guid Id { get; }
    public ClientId ClientId { get; }
    public AccountId AccountId { get; }
    public SecurityCode SecurityCode { get; }
    public Quantity Quantity { get; }
    public OrderStatus Status { get; private set; }

    public Order(
        ClientId clientId,
        AccountId accountId,
        SecurityCode securityCode,
        Quantity quantity)
    {
        Id = Guid.NewGuid();
        ClientId = clientId;
        AccountId = accountId;
        SecurityCode = securityCode;
        Quantity = quantity;
        Status = OrderStatus.Created;
    }

    public void MarkReserved()
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Only created order can be reserved");
        Status = OrderStatus.Reserved;
    }

    public void MarkRejected()
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Only created order can be rejected");

        Status = OrderStatus.Rejected;
    }
}