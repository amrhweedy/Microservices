
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Application.Data;
using Ordering.Infrastructure.Interceptors;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("Database");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, opts) =>
        {
            opts.UseSqlServer(connectionString);
            opts.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());   // Addinterceptors >>  take objects from the interceptor classes so the sp.GetServices<ISaveChangesInterceptor>() will give me objects from all services which implement the ISaveChangesInterceptor and make register for them 
        });

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();


        return services;
    }
}
