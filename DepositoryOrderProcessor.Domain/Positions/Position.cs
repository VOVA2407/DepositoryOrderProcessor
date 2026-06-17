using DepositoryOrderProcessor.Domain.ValueObjects;

namespace DepositoryOrderProcessor.Domain.Positions;

public sealed class Position
{
    public AccountId AccountId { get; }
    public SecurityCode SecurityCode { get; }
    public decimal TotalQuantity { get; private set; }
    public decimal ReservedQuantity { get; private set; }

    public decimal AvailableQuantity => TotalQuantity - ReservedQuantity;

    public Position(
        AccountId accountId,
        SecurityCode securityCode,

        decimal totalQuantity)
    {
        if (totalQuantity < 0)
            throw new ArgumentException("Total quantity cannot be negative.");

        AccountId = accountId;
        SecurityCode = securityCode;
        TotalQuantity = totalQuantity;
        ReservedQuantity = 0;
    }

    public bool CanReserve(Quantity quantity)
    {
        return AvailableQuantity >= quantity.Value;
    }

    public void Reserve(Quantity quantity)
    {
        if (!CanReserve(quantity))
            throw new InvalidOperationException("Not enough avaliable quantity");

        ReservedQuantity += quantity.Value;
    }
    
}
