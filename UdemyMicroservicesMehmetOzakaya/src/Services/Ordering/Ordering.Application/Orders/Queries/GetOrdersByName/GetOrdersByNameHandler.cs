using Microsoft.EntityFrameworkCore;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrdersByName;
public class GetOrdersByNameHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersByNameQuery, GetOrderByNameResult>
{
    public async Task<GetOrderByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
                   .Include(o => o.OrderItems)
                   .AsNoTracking()
                   .Where(o => o.OrderName.Value.Contains(query.Name))
                   .OrderBy(o => o.OrderName)
                   .ToListAsync();


        return new GetOrderByNameResult(orders.ToOrderDtoList());

    }

}
