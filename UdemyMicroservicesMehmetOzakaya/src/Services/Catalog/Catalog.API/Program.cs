

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
    //opts.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.All; 
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>(); // to seeding data

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);   // make health for the application and the postgress database to ensure that they work well

var app = builder.Build();


app.MapCarter();
app.UseExceptionHandler(options=> { }); // we configure the application to use our custom exception handler
                                        // so the empty option parameter indicates that we are relying on custom configured handler


//https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks  we download the 2 packages related to the health from this repo to make health check for the postgress and to show the health check response in a json not string(raw)
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}) ;
app.Run();
