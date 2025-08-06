using Ordering.Application.Extentions;

namespace Ordering.Application.Orders.Queries.GetOrdersByName
{
    public class GetOrdersByNameHandler(IApplicationDbContext dbContext)
        : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameQueryResult>
    {
        public async Task<GetOrdersByNameQueryResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
        {
            // get orders by name using dbContext
            // return result
            var orders = await dbContext.Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .Where(o => o.OrderName.Value.Contains(query.Name))
                .OrderBy(o => o.OrderName)
                .ToListAsync(cancellationToken);

            // var orderDtos = ProjectToOrderDto(orders); // Antes de usar la extensión
            var orderDtos = orders.ToOrderDtoList();
            return new GetOrdersByNameQueryResult(orderDtos);
        }

        private List<OrderDto> ProjectToOrderDto(List<Order> orders)
        {
            List<OrderDto> result = new();
            foreach (var order in orders)
            {
                var orderDto = new OrderDto(
                    Id: order.Id.Value,
                    CustomerId: order.CustomerId.Value,
                    OrderName: order.OrderName.Value,
                    ShippingAddress: new AddressDto(
                        order.ShippingAddress.FirstName,
                        order.ShippingAddress.LastName,
                        order.ShippingAddress.EmailAddress,
                        order.ShippingAddress.AddressLine,
                        order.ShippingAddress.State,
                        order.ShippingAddress.ZipCode,
                        order.ShippingAddress.Country),
                    BillingAddress: new AddressDto(
                        order.BillingAddress.FirstName,
                        order.BillingAddress.LastName,
                        order.BillingAddress.EmailAddress,
                        order.BillingAddress.AddressLine,
                        order.BillingAddress.State,
                        order.BillingAddress.ZipCode,
                        order.BillingAddress.Country),
                    Payment: new PaymentDto(
                        order.Payment.CardName,
                        order.Payment.CardNumber,
                        order.Payment.Expiration,
                        order.Payment.CVV,
                        order.Payment.PaymentMethod),
                    Status: order.Status,
                    OrderItems: order.OrderItems.Select(oi => new OrderItemDto(
                        OrderId: oi.OrderId.Value,
                        ProductId: oi.ProductId.Value,
                        Quantity: oi.Quantity,
                        Price: oi.Price)).ToList()
                );

                result.Add(orderDto);
            }

            return result;
        }

        public Task<GetOrdersByNameQueryResult> Handle(GetOrdersByNameQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
