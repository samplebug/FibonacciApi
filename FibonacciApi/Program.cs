using FibonacciApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<FibonacciService>();
builder.Services.AddControllers();
builder.Services.AddRazorPages();
var app = builder.Build();
app.UseRouting();
app.MapRazorPages();
app.MapControllers();
app.Run();