using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id); // Indica que Id es la clave primaria

            builder.Property(oi => oi.Id)
                .HasConversion(
                    orderItemId => orderItemId.Value, // Convertimos OrderItemId a Guid para almacenarlo
                    dbId => OrderItemId.Of(dbId)); // Convertimos Guid a OrderItemId al leerlo

            // Configurando la relacion
            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            builder.Property(oi => oi.Quantity).IsRequired();

            builder.Property(oi => oi.Price).IsRequired();
        }
    }
}
