
using Discount.GRPC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();

builder.Services.AddMediatR(config =>
{
    // scan the assembly to find the all request handlers and register them
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);    // userName is primary key
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

// we use the Scrutor library to register the CashedBasketRepository
//After applying Scrutor's Decorate method, any class that injects IBasketRepository will receive an instance of CachedBasketRepository. This allows CachedBasketRepository to act as a decorator, adding caching behavior on top of the original BasketRepository functionality.

builder.Services.Decorate<IBasketRepository, CashedBasketRepository>();

// this is a solution to register the CashedBasketRepository manually but it is not the best solution  
//builder.Services.AddScoped<IBasketRepository>(provider =>
//{
//    var basketRepository = provider.GetRequiredService<BasketRepository>();
//    return new CashedBasketRepository(basketRepository,provider.GetRequiredService<IDistributedCache>());
//});


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!) // make health for the application and the postgress database to ensure that they work well
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);


builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opts =>
{
    opts.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
}).ConfigurePrimaryHttpMessageHandler(() =>   // configure ssl certificate (important when run the containers)
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
    return handler;
});

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler(opts => { });

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.Run();
