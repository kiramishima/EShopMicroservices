﻿namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price): ICommand<CreateProductResult>;


// Validation class
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}

public record CreateProductResult(Guid Id);

// Application Login Layer
internal class CreateProductCommandHandler
    (IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Business logic to create a product.
        // save to database
        // return CreateProductResult result

        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price,
        };
        // TODO
        // save to database
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        // return result
        return new CreateProductResult(product.Id);
    }
}

