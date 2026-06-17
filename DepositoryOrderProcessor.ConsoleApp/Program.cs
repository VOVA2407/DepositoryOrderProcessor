using DepositoryOrderProcessor.Application.Abstractions;
using DepositoryOrderProcessor.Application.Orders;
using DepositoryOrderProcessor.Domain.Orders;
using DepositoryOrderProcessor.Domain.ValueObjects;
using DepositoryOrderProcessor.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddSingleton<IPositionRepository, InMemoryPositionRepository>();
services.AddTransient<OrderProcessor>();

var serviceProvider = services.BuildServiceProvider();

var orderProcessor = serviceProvider.GetRequiredService<OrderProcessor>();
var positionRepository = serviceProvider.GetRequiredService<IPositionRepository>();

var clientId = new ClientId(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));
var accountId = new AccountId(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
var securityCode = new SecurityCode("SBER");

var order = new Order(
    clientId,

    accountId,
    securityCode,
    new Quantity(7)
    );

var positionBefore = positionRepository.Find(accountId, securityCode);

Console.WriteLine($"Создано поручение: Reserve {order.Quantity} {order.SecurityCode.Value}");
Console.WriteLine($"Свободный остаток до обработки: {positionBefore?.AvailableQuantity}");

orderProcessor.Process(order);

var positionAfter = positionRepository.Find(accountId, securityCode);

Console.WriteLine($"Статус поручения:{order.Status}");
Console.WriteLine($"Свободный остаток после обработки: {positionAfter?.AvailableQuantity}");

var order2 = new Order(
    clientId,

    accountId,
    securityCode,
    new Quantity(15)
    );

orderProcessor.Process(order2);

var positionAfterSecondOrder = positionRepository.Find(accountId, securityCode);

Console.WriteLine($"Статус поручения:{order2.Status}");
Console.WriteLine($"Свободный остаток после обработки: {positionAfterSecondOrder?.AvailableQuantity}");