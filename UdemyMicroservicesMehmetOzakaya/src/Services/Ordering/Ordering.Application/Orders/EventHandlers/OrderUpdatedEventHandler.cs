﻿
namespace Ordering.Application.Orders.EventHandlers;
public class OrderUpdatedEventHandler(ILogger<OrderUpdatedEvent> logger) : INotificationHandler<OrderUpdatedEvent>
{
    public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event Handler : {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
