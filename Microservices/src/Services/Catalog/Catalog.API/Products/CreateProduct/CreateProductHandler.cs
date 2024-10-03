

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("category is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("image file is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("price must be greater than 0");
    }
}
public class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        //var result = await validator.ValidateAsync(command, cancellationToken);
        //var errors = result.Errors.Select(error => error.ErrorMessage).ToList();
        //if (errors.Any())
        //{
        //    throw new ValidationException(errors.FirstOrDefault());
        //}

        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price,
        };

        // todo >>> save database
        session.Store(product); // if there is no table products in the database the marten library will create it automatically
        await session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}
