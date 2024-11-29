using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins("http://127.0.0.1:5500")
			   .AllowAnyHeader()
			   .AllowAnyMethod()
			   .AllowCredentials();
        });
    });


var app = builder.Build();

app.UseCors(); // Должно быть перед UseEndpoints

app.MapHub<GameHub>("/game");

app.Run();
