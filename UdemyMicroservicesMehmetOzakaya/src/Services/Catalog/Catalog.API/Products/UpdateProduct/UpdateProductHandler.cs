﻿
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id,string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price): ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("id is required");

        RuleFor(x => x.Name).NotEmpty().WithMessage("name is required")
            .Length(2, 150).WithMessage("name must be between 2 and 150 characters");

        RuleFor(x => x.Price).GreaterThan(0).WithMessage("price must be greater than 0");
    }
}

public class UpdateProductCommandHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product == null)
        {
            throw new ProductNotFoundException();
        }

        product.Name = command.Name;
        product.Category = command.Category;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;  
        product.Price = command.Price;

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
        return new UpdateProductResult(true);
    }

}