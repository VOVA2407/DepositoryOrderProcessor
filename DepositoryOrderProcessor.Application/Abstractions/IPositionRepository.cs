using DepositoryOrderProcessor.Domain.Positions;
using DepositoryOrderProcessor.Domain.ValueObjects;

namespace DepositoryOrderProcessor.Application.Abstractions;
public interface IPositionRepository
{
    Position? Find(AccountId accountId, SecurityCode securityCode);
}