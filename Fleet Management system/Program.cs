using Microsoft.EntityFrameworkCore;
using Fleet_Management_system.Data;
using Microsoft.AspNetCore.Builder;
using Fleet_Management_system.WebSocket;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://localhost:7276");

builder.Services
    .AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .WithMethods("GET", "POST", "PUT", "DELETE")
                .AllowCredentials();
        });
});

builder.Services
    .AddDbContext<Contextdata>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<WebSocketManagerService>();

var app = builder.Build();
app.UseCors();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var webSocketManagerService = app.Services.GetRequiredService<WebSocketManagerService>();
webSocketManagerService.Start("ws://0.0.0.0:8181");

app.Run();
