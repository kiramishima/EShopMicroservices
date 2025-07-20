using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

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
// Decorando manualmente, pero no es tanm escalable como usar un contenedor IoC
//builder.Services.AddScoped<IBasketRepository>(provider =>
//{
//    var basketRepository = provider.GetRequiredService<BasketRepository>();
//    return new CachedBasketRepository(basketRepository, provider.GetRequiredService<IDistributedCache>());
//});
// usamos Scrutor para decorar automáticamente
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
// Registramos IDistributedCache para Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis")!;
    // options.InstanceName = "Basket";
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>(); // Register the custom exception handler

// Health checks
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();
app.UseExceptionHandler(opts => {});

// Health checks endpoint
app.UseHealthChecks("/health", 
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
