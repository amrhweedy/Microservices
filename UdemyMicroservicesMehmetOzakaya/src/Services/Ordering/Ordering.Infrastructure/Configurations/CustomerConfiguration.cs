using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Configurations;
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasConversion(
               customerId => customerId.Value,   // it specifies how to convert the "customerId" value object to its underlying Guid value when saving the entity to the database Essentially, it extracts the Guid value from the CustomerId object.
                dbId => CustomerId.Of(dbId) // This lambda expression specifies how to convert the Guid value retrieved from the database back to the CustomerId value object when loading the entity from the database. It creates a new CustomerId instance using the Of method, which includes validation logic to ensure the Guid is valid.
            );

        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(255);
        builder.HasIndex(c => c.Email).IsUnique();
    }
}
