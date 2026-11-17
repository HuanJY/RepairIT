using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RepairIT.Client;
using RepairIT.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


var adresseGateway = new Uri(builder.Configuration["GatewayUrl"] ?? "http://localhost:5010/");

builder.Services.AddHttpClient<IOrdinateurApiService, OrdinateurApiService>(client => client.BaseAddress = adresseGateway);
builder.Services.AddHttpClient<IUtilisateurApiService, UtilisateurApiService>(client => client.BaseAddress = adresseGateway);

await builder.Build().RunAsync();
