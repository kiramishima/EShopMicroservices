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

            var orderDtos = orders.ToOrderDtoList();
            return new GetOrdersByNameQueryResult(orderDtos);
        }

        public Task<GetOrdersByNameQueryResult> Handle(GetOrdersByNameQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
