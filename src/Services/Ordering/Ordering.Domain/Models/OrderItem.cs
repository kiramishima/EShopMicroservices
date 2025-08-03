﻿namespace Ordering.Domain.Models
{
    public class OrderItem : Entity<OrderItemId>
    {
        public OrderId OrderId { get; private set; }
        public ProductId ProductId { get; private set; } = default!;
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        internal OrderItem(OrderId orderId, ProductId productId, int quantity, decimal price)
        {
            Id = OrderItemId.Of(Guid.NewGuid());
            OrderId = orderId;
            ProductId = productId;
            Price = price;
            Quantity = quantity;
        }
    }
}
