using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register application services here
            // Example:
            //services.AddMediatR(cfg => {
            //  cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //}
            return services;
        }
    }
}
