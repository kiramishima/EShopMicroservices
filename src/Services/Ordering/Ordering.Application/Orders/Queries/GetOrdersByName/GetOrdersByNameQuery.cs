namespace Ordering.Application.Orders.Queries.GetOrdersByName
{
    public record GetOrdersByNameQuery(string Name)
        : IQuery<GetOrdersByNameQueryResult>;

    public record GetOrdersByNameQueryResult(IEnumerable<OrderDto> Orders);
}
