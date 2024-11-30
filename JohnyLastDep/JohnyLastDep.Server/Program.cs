using JohnyLastDep.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(builder =>
	{
		builder.WithOrigins("https://localhost:7227")
		   .AllowAnyHeader()
		   .AllowAnyMethod()
		   .AllowCredentials();
	});
});


var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors(); // Должно быть перед UseEndpoints

app.MapHub<GameHub>("/game");

app.Run();