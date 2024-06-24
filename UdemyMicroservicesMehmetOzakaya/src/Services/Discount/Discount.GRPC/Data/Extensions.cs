using Microsoft.EntityFrameworkCore;

namespace Discount.GRPC.Data;

public static  class Extensions
{

    public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
    {
         using var scope = app.ApplicationServices.CreateScope();
         using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountContext>();

        // automate applying the migrations in the database without write the command update database 
        // this is the best solution because if i make container for the application and i need to apply the migrations in the database, i cannot open the container in the production and the write the command (update-database)

        dbContext.Database.MigrateAsync(); 
        return app;
    }
}
