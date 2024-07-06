using BuildingBlocks.Messaging.Events;
using MassTransit;
using Ordering.Application.Orders.Commands.CreateOrder;
using Ordering.Domain.Enums;

namespace Ordering.Application.Orders.EventHandlers.Integration;
public class BasketCheckoutEventHandler(ILogger<BasketCheckoutEventHandler> logger, ISender sender)
    : IConsumer<BasketCheckoutEvent>  // consume the BasketCheckoutEvent  which is published from the basket service
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        // TODO: Create new order and start order fullfillment process

        logger.LogInformation("Integration Event Handled : {IntegrationEvent}", context.Message.GetType().Name);


        // in this step will receive the message or the event and use the event data to create the order
        // if you open the CreateOrderHandler you will find that we will call function in the ORDER domain "create function" which will create object from the order and publish the "CreatedOrderDomainEvent" which is handled by "OrderCreatedEventHandler"
        var command = MapToCreateOrderCommand(context.Message);
        await sender.Send(command);  // will send the command to the CreateOrderHandler


    }

    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
    {
        var addressDto = new AddressDto(message.FirstName, message.LastName, message.EmailAddress, message.AddressLine, message.Country, message.State, message.ZipCode);
        var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.Expiration, message.CVV, message.PaymentMethod);
        var orderId = Guid.NewGuid();

        var order = new OrderDto(
            Id: orderId,
            CustomerId: message.CustomerId,
            OrderName: message.UserName,
            ShippingAddress: addressDto,
            BillingAddress: addressDto,
            Payment: paymentDto,
            Status: OrderStatus.Pending,
            OrderItems: [

                new OrderItemDto(orderId, new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"), 2, 500),
                new OrderItemDto(orderId, new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"), 1, 400)
                ]

            );

        return new CreateOrderCommand(order);

    }
}
