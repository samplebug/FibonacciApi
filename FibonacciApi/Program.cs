using FibonacciApi;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
             .WriteTo.Console()
             .WriteTo.File("logs/fibonacci.log", rollingInterval: RollingInterval.Day)
             .CreateLogger();
builder.Host.UseSerilog();

// Add Redis Distributed Cache
// builder.Services.AddStackExchangeRedisCache(
//    options =>
//    {
//        options.Configuration = "localhost:6363";
//        options.InstanceName = "FibonacciCache_";
//    });
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IFibonacciService, FibonacciService>();
builder.Services.AddControllers();
builder.Services.AddRazorPages();  // just for the sake of it

var app = builder.Build();
app.UseRouting();
app.UseHttpMetrics();
app.MapMetrics();     // expose the /metrics endpoint
app.MapControllers(); // API routes
app.MapRazorPages();  // UI
app.Run();