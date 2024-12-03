using JohnyLastDep.UI;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp =>
{
	var navigationManager = sp.GetRequiredService<NavigationManager>();

	var hubConnection = new HubConnectionBuilder()
		.WithUrl("https://localhost:7144/game")
		.WithAutomaticReconnect()
		.Build();

	hubConnection.StartAsync();

	return hubConnection;
});

builder.Services.AddSingleton<GameClient>();

await builder.Build().RunAsync();
