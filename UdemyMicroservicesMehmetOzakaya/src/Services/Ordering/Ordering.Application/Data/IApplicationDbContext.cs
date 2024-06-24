using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;

namespace Ordering.Application.Data;
public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<Product> Products { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

}


// we make this because the order application layer needs the ApplicationDbContext but the ApplicationDbContext in the infrastructure layer and based on the clean architecture the application layer does not depend on the infrastructure layer
// so we make this interface and make the ApplicationDbContext implement it and register this interface and the ApplicationDbContext in the program.cs
// so when we inject this interface in any class we will get an object from the ApplicationDbContext
// this interface is an abstraction over the ApplicationDbContext
// we make this because we need the application layer does not depend on any database in the infrastructure layer