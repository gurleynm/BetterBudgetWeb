using BetterBudgetWeb;
using BetterBudgetWeb.Simulation;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://betterbudgetapi.azurewebsites.net") });

builder.Services.AddBlazoredSessionStorage();

await Constants.Init();

await builder.Build().RunAsync();
