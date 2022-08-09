using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseWebSockets();
app.Use(async (ctx, next) =>
{
    WriteRequestParam(ctx);
    if (ctx.WebSockets.IsWebSocketRequest)
    {
        var websocket = await ctx.WebSockets.AcceptWebSocketAsync();
        Console.WriteLine("Websocket connected");
    }
    else
    {
        Console.WriteLine("Hello from the 2rd request delegate.");
        await next();
    }
});

app.Run(async (ctx) =>
{
    var text = "Hello from the 3rd request delegate.";
    Console.WriteLine(text);
    await ctx.Response.WriteAsync(text);
});

app.Run();

void WriteRequestParam(HttpContext context)
{
    Console.WriteLine($"Request method: {context.Request.Method}");
    Console.WriteLine($"Request protocol: {context.Request.Protocol}");

    if (context.Request.Headers is null) return;
    
    foreach (var header in context.Request.Headers)
    {
        Console.WriteLine($"--> {header.Key} : {header.Value}");
    }
}