var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<Program>();
    // alternative
    // config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    // opts.AutoCreateSchemaObjects = AutoCreate.All; // for development only, use AutoCreate.None in production
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName); // indicamos que queremos usar UserName como ID
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>(); // Inject the repository

builder.Services.AddExceptionHandler<CustomExceptionHandler>(); // Register the custom exception handler

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();
app.UseExceptionHandler(opts => {});

app.Run();
