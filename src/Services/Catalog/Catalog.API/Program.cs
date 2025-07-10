var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCarter(); // DI Carter
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    // config.RegisterServicesFromAssemblyContaining<Program>();
}); // DI MediatR

builder.Services.AddValidatorsFromAssembly(assembly); // Add FluentValidation

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions(); // Add Marten

// Add custom exception handler
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter(); // map carter

// Removimos nuestro middleware de manejo de excepciones personalizado
// Agregamos el CustomExceptionHandler que creamos en BuildingBlocks/Exceptions/Handler/CustomExceptionHandler.cs
// Ya que es mas eficiente y reutilizable para proyectos grandes
app.UseExceptionHandler(options =>
{

});

app.Run();
