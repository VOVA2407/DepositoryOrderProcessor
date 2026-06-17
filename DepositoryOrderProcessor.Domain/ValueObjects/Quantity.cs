namespace DepositoryOrderProcessor.Domain.ValueObjects;
public readonly record struct Quantity
{
    public decimal Value { get; }

    public Quantity(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        Value = value;
    }

    public override string ToString() => Value.ToString();
}