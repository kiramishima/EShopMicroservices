using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id); // indicamos que Id es la clave primaria que viene del modelo
            builder.Property(c => c.Id).HasConversion(
                    customerId => customerId.Value, // Convertimos CustomerId a Guid para almacenarlo
                    dbId => CustomerId.Of(dbId)); // Convertimos Guid a CustomerId al leerlo

            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
            
            builder.Property(c => c.Email).HasMaxLength(255);

            builder.HasIndex(c => c.Email).IsUnique();
        }
    }
}
