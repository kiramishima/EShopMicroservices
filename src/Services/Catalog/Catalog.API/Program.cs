var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCarter(); // DI Carter
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    // config.RegisterServicesFromAssemblyContaining<Program>();
}); // DI MediatR
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions(); // Add Marten

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter(); // map carter

app.Run();
