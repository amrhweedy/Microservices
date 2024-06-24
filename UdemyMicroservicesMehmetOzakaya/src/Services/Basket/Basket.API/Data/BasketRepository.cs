
namespace Basket.API.Data;

public class BasketRepository(IDocumentSession session) : IBasketRepository
{
   
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var shoppingCart = await session.LoadAsync<ShoppingCart>(userName,cancellationToken);
        return shoppingCart is null ? throw new BasketNotFoundException(userName) : shoppingCart;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        session.Store(basket);   // upsert
        await session.SaveChangesAsync(cancellationToken);
        return basket;

    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        session.Delete<ShoppingCart>(userName);
        await session.SaveChangesAsync(cancellationToken);
        return true;
    }
}
