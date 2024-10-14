using BetterBudgetWeb;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Stripe;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api.couplesbetterbudget.com") });

builder.Services.AddBlazoredSessionStorage();
StripeConfiguration.ApiKey = HiddenEnv.APIKey;

Constants.SetColorScheme();

await builder.Build().RunAsync();
