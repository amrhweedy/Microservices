namespace Ordering.Application.Orders.Commands.CreateOrder;
public class CreateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = CreateNewOrder(command.Order);

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateOrderResult(order.Id.Value);
    }

    private Order CreateNewOrder(OrderDto OrderDto)
    {
        var shippingAddress = Address.Of(OrderDto.ShippingAddress.FirstName, OrderDto.ShippingAddress.LastName, OrderDto.ShippingAddress.EmailAddress, OrderDto.ShippingAddress.AddressLine, OrderDto.ShippingAddress.Country, OrderDto.ShippingAddress.State, OrderDto.ShippingAddress.ZipCode);
        var billingAddress = Address.Of(OrderDto.BillingAddress.FirstName, OrderDto.BillingAddress.LastName, OrderDto.BillingAddress.EmailAddress, OrderDto.BillingAddress.AddressLine, OrderDto.BillingAddress.Country, OrderDto.BillingAddress.State, OrderDto.BillingAddress.ZipCode);

        var order = Order.Create(
            id: OrderId.Of(Guid.NewGuid()),
            customerId: CustomerId.Of(OrderDto.CustomerId),
            orderName: OrderName.Of(OrderDto.OrderName),
            shippingAddress: shippingAddress,
            billingAddress: billingAddress,
            payment: Payment.Of(OrderDto.Payment.CardName, OrderDto.Payment.CardNumber, OrderDto.Payment.Expiration, OrderDto.Payment.Cvv, OrderDto.Payment.PaymentMethod)
            );

        foreach (var orderItemDto in OrderDto.OrderItems)
        {
            order.Add(ProductId.Of(orderItemDto.ProductId), orderItemDto.Quantity, orderItemDto.Price);
        }

        return order;

    }
}
