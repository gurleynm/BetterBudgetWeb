using BetterBudgetWeb;
using Blazored.SessionStorage;
using GoogleCaptchaComponent;
using GoogleCaptchaComponent.Configuration;
using GoogleCaptchaComponent.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Stripe;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api.couplesbetterbudget.com") });
builder.Services.AddBlazorBootstrap();

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddGoogleCaptcha(configuration =>
{
    configuration.V2SiteKey = "6LccOnMrAAAAAG-E6OCLaeN9TKAGKeCenhEuZ43L";
    configuration.V3SiteKey = "6LccOnMrAAAAAG-E6OCLaeN9TKAGKeCenhEuZ43L";
    configuration.DefaultVersion = CaptchaConfiguration.Version.V2;
    configuration.DefaultTheme = CaptchaConfiguration.Theme.Light;
    configuration.DefaultLanguage = CaptchaLanguages.English;
});

StripeConfiguration.ApiKey = HiddenEnv.APIKey;

Constants.SetColorScheme();

await builder.Build().RunAsync();
