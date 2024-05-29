<<<<<<< Updated upstream
using Microsoft.EntityFrameworkCore;
using Fleet_Management_system.Data;
using Fleet_Management_system;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("https://localhost:7276");

builder.Services.AddControllersWithViews()
=======
﻿using Microsoft.EntityFrameworkCore;
using Fleet_Management_system.Data;
using Microsoft.AspNetCore.Builder;
using Fleet_Management_system.WebSocket;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://localhost:7276");

builder.Services
    .AddControllersWithViews()
>>>>>>> Stashed changes
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });
<<<<<<< Updated upstream
builder.Services.AddDbContext<Contextdata>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

app.UseWebSockets(webSocketOptions);

app.Use(async (context, next) =>
{
    Console.WriteLine("Incoming request: " + context.Request.Path);
    await next.Invoke();
});
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await WebSocketUtilities.Echo(webSocket);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
    else
    {
        await next(context);
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthorization();
=======
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

>>>>>>> Stashed changes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

<<<<<<< Updated upstream

await app.RunAsync();
=======
var webSocketManagerService = app.Services.GetRequiredService<WebSocketManagerService>();
webSocketManagerService.Start("ws://0.0.0.0:8181");

app.Run();
>>>>>>> Stashed changes
