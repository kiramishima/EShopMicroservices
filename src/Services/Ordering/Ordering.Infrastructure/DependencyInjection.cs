using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");
            // Register infrastructure services here
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp, options) => { // sp = service provider
                // Agregando interceptor para el manejo de transacciones
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>()); // de esta manera podemos acdecer a a Mediator
                options.UseSqlServer(connectionString);
            });

            // services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            return services;
        }
    }
}
