using System.Reflection;
using Ordering.Application.Data;

namespace Ordering.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // Entities
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Sobreescribimos el método OnModelCreating para configurar las entidades
            // builder.Entity<Customer>().Property(c => c.Name).IsRequired().HasMaxLength(100); // Vamos a hacerlo en archivos separados por grupos
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // para configurar por separado las entidades

            base.OnModelCreating(builder);
        }
    }
}
