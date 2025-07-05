namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price);

public record CreateProductResponse(Guid Id);

// Presentation Logic Layer
public class CreateProductEndpoint : ICarterModule // CarterModule is used to define routes in Carter
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
        {
            // Map the request to a command
            var command = request.Adapt<CreateProductCommand>();
            // Send the command
            var result = await sender.Send(command);

            var response = result.Adapt<CreateProductResponse>();
            // Map the result to a response
            return Results.Created($"/products/{response.Id}", response);
        })
        .WithName("CreateProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Product")
        .WithDescription("Create Product");
    }
}

