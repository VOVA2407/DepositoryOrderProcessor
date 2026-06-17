using DepositoryOrderProcessor.Application.Abstractions;
using DepositoryOrderProcessor.Application.Orders;
using DepositoryOrderProcessor.Domain.Orders;
using DepositoryOrderProcessor.Domain.ValueObjects;
using DepositoryOrderProcessor.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPositionRepository, InMemoryPositionRepository>();
builder.Services.AddTransient<OrderProcessor>();

var app = builder.Build();

app.MapGet("/", () => "Depository Order Processor API is running");

app.MapGet("/positions/{accountId:guid}/{securityCode}", (

    Guid accountId,

string securityCode,
IPositionRepository positionRepository) =>
{
    var position = positionRepository.Find(
    new AccountId(accountId),
    new SecurityCode(securityCode));

    if (position is null)
    {
        return Results.NotFound(new
        {
            message = "Position not found"
        });
    }

    return Results.Ok(new PositionResponse(
        position.AccountId.Value,
        position.SecurityCode.Value,
        position.TotalQuantity,
        position.ReservedQuantity,
        position.AvailableQuantity
    ));
});

app.MapPost("/orders/reserve", (
    ReservedOrderRequest request,
    OrderProcessor orderProcessor) =>
{
    if (request.Quantity <= 0)
    {
        return Results.BadRequest(new
        {
            message = "Quantiry must be greate than zero"
        });
    }

    if (string.IsNullOrWhiteSpace(request.SecurityCode))
    {
        return Results.BadRequest(new
        {
            message = "SecurityCode is required"
        });
    }

    var order = new Order(
    new ClientId(request.ClientId),
    new AccountId(request.AccountId),
    new SecurityCode(request.SecurityCode),
    new Quantity(request.Quantity));

    var result = orderProcessor.Process(order);

    var response = new ReserveOrderResponse(
        result.OrderId,
        result.Status.ToString(),
        result.AvailableQuantity,
        result.RejectionReason);

    return Results.Ok(response);
});

app.Run();
public sealed record ReservedOrderRequest(
Guid ClientId,
Guid AccountId,
string SecurityCode,
decimal Quantity
);

public sealed record ReserveOrderResponse(
Guid OrderId,
string Status,
decimal? AvailableQuantity,
string? RejectionReason
);

public sealed record PositionResponse(
Guid AccountId,
string SecurityCode,
decimal TotalQuantity,
decimal ReservedQuantity,
decimal AvailableQuantity
);



