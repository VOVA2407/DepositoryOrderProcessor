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

    public OrderProcessingResult Process(Order order)
    {
        var position = _positionRepository.Find(order.AccountId, order.SecurityCode);

        if (position is null)
        {
            order.MarkRejected();
            return new OrderProcessingResult(
                order.Id,
                order.Status,
                AvailableQuantity: null,
                RejectionReason: "PositionNotFound"
            );
        }

        if (!position.CanReserve(order.Quantity))
        {
            order.MarkRejected();

            return new OrderProcessingResult(
                order.Id,
                order.Status,
                position.AvailableQuantity,
                RejectionReason: "NotEnoughAvailableQuantity"
            );
        }

        position.Reserve(order.Quantity);
        order.MarkReserved();

        return new OrderProcessingResult(
            order.Id,
            order.Status,
            position.AvailableQuantity,
            RejectionReason: null
        );
    }
}