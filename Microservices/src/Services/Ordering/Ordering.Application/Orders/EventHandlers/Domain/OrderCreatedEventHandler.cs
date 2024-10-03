using MassTransit;
using Microsoft.FeatureManagement;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.EventHandlers.Domain;
public class OrderCreatedEventHandler(IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> logger) : INotificationHandler<OrderCreatedEvent>
{
    // first >>> we receive orderDto then convert it to order to create the order and save this order in the database and publish domain event (OrderCreatedEvent)
    // second >> here we receive OrderCreatedEvent which contains Order then convert this order to orderDto to publish integration event to rabbitMQ with this orderDto
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event Handler : {DomainEvent}", domainEvent.GetType().Name);

        // we use the feature management here, because this method is triggered or executed when the order is created (when Order.Create) is called
        // so there is seeding orders when the application starts running so because of this seeding data this method will be triggered and there are messages will be send to the rabbitMQ and there are no subscribers for them
        // so we use the feature management to enable or disable this feature based on condition in the app.settings.json
        if (await featureManager.IsEnabledAsync("FeatureManagement"))
        {
            var orderCreatedIntegrationEvent = domainEvent.order.ToOrderDto();

            await publishEndpoint.Publish(orderCreatedIntegrationEvent);

        }

    }
}
