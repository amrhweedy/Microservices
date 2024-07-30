using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
         .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
         .AddEnvironmentVariables();


builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    //you're adding a specific type of rate limiter called a Fixed Window Limiter. This limiter allows a certain number of requests (the permit limit) in a fixed time window.
    // The string "fixed" is an identifier for this limiter, which you can reference later in your application if needed.
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });

});
var app = builder.Build();

// configure the http request pipelines

app.UseRateLimiter();

app.MapReverseProxy();

app.Run();
