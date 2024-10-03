using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Catalog;

namespace Shopping.Web.Pages;
public class IndexModel(ICatalogService catalogService, IBasketService basketService, ILogger<IndexModel> logger) : PageModel
{
    public IEnumerable<ProductModel> ProductList { get; set; } = new List<ProductModel>();



    public async Task<IActionResult> OnGetAsync()  // it will be triggered or executed when i visit the index page in the browser , it will send the request to the yarp gateway to get all products
    {
        logger.LogInformation("index page visited");
        var result = await catalogService.GetProducts();
        ProductList = result.Products;
        return Page();

    }

    public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
    {
        logger.LogInformation("add to cart button clicked");
        var productResponse = await catalogService.GetProduct(productId);
        var basket = await basketService.LoadUserBasket();
        basket.Items.Add(new ShoppingCartItemModel
        {

            ProductId = productId,
            ProductName = productResponse.Product.Name,
            Price = productResponse.Product.Price,
            Quantity = 1,
            Color = "Black"
        });

        await basketService.StoreBasket(new StoreBasketRequest(basket));
        return RedirectToPage("Cart");
    }
}
