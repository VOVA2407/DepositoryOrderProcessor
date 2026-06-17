using DepositoryOrderProcessor.Application.Abstractions;
using DepositoryOrderProcessor.Domain.Positions;
using DepositoryOrderProcessor.Domain.ValueObjects;
using DepositoryOrderProcessor.Infrastructure.Repositories;

namespace DepositoryOrderProcessor.Infrastructure.Repositories;

public sealed class InMemoryPositionRepository : IPositionRepository
{
    private readonly List<Position> _positions;

    public InMemoryPositionRepository()
    {
        var accountId = new AccountId(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
        var securityCode = new SecurityCode("SBER");

        _positions = new List<Position>
        {
            new Position(accountId, securityCode, 10)
        };
    }

    public Position? Find(AccountId accountId, SecurityCode securityCode)
    {
        return _positions.FirstOrDefault(position =>
            position.AccountId == accountId &&
            position.SecurityCode == securityCode);
    } 
}