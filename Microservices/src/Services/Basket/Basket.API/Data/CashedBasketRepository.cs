
using JasperFx.Core;
using StackExchange.Redis;

namespace Basket.API.Data;
// the CashedBasketRepository is implements the proxy pattern and the decorator pattern 
//Proxy Pattern Implementation
   //Control Access: The CachedBasketRepository controls access to the BasketRepository by checking the cache before calling the underlying repository methods.
   //Add Behavior: The CachedBasketRepository adds behavior such as caching and retrieving data from the cache.
//Decorator Pattern Implementation
   //Wrapping: The CachedBasketRepository wraps the BasketRepository, implementing the same IBasketRepository interface.
   //Enhancing Functionality: The CachedBasketRepository enhances the functionality by adding caching logic before and after calling the methods of BasketRepository.

public class CashedBasketRepository(IBasketRepository repository, IDistributedCache cashe) : IBasketRepository
{

    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var cashedBasket = await cashe.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cashedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cashedBasket)!; // convert from string(json) to object

        var basket = await repository.GetBasket(userName, cancellationToken);
        await cashe.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.StoreBasket(basket, cancellationToken);
        await cashe.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
         await repository.DeleteBasket(userName, cancellationToken);
        await cashe.RemoveAsync(userName, cancellationToken);
        return true;
    }

}
