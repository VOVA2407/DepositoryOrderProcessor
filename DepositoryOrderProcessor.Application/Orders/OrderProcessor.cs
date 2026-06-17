using DepositoryOrderProcessor.Application.Abstractions;
using DepositoryOrderProcessor.Domain.Orders;

namespace DepositoryOrderProcessor.Application.Orders;

public sealed class OrderProcessor
{
    private readonly IPositionRepository _positionRepository;
    public OrderProcessor(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    }

    public void Process(Order order)
    {
        var position = _positionRepository.Find(order.AccountId, order.SecurityCode);

        if (position is null)
        {
            order.MarkRejected();
            return;
        }

        if (!position.CanReserve(order.Quantity))
        {
            order.MarkRejected();
            return;
        }

        position.Reserve(order.Quantity);
        order.MarkReserved();
    }
}