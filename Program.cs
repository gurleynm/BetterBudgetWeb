using BetterBudgetWeb;
using BetterBudgetWeb.MonthView;
using BetterBudgetWeb.Services;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Stripe;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api.couplesbetterbudget.com") });
builder.Services.AddScoped<IPaymentService,PaymentService>();
builder.Services.AddBlazoredSessionStorage();
StripeConfiguration.ApiKey = "sk_test_51Q2DNf2L4K66u9tvoLfr7puGwu0fitJyRyOb2cYDe5ndUNkr6BVX2HJF5naq8nt1bGQ5CxKoXB5PiCQgAMlegr0Y00JZQeBnjN";

await Constants.Init();

await builder.Build().RunAsync();
