namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);
public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
        {
            // 1- convert request to command
            var command = request.Adapt<CreateProductCommand>();

            //2- send the request and receives the result
            var result = await sender.Send(command);

            //3- convert the result from mediator to the response 
            var response = result.Adapt<CreateProductResponse>();

            // 4- it return the response and  after create the product will get this product by id 
            return Results.Created($"/products/{response.Id}", response);
        })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("CreateProduct")
            .WithDescription("CreateProduct");
    }
}
