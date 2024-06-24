using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// builder.Services >> services property return object implementing the iServiceCollection so it can access or call any methods in this interface
// the 3 extension methods extend the IServiceCollection so the Services property (which return object ) can call or access these 3 extension methods 

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);


var app = builder.Build();

app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
}


app.Run();

