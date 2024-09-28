using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BetterBudgetWeb.Data
{
    public class CustomErrorBoundary : ErrorBoundary
    {
        [Inject]
        private IWebAssemblyHostEnvironment env { get; set; }

        protected override Task OnErrorAsync(Exception exception)
        {
            if (env.IsDevelopment())
            {
                string c = exception.StackTrace;
                return base.OnErrorAsync(exception);
            }
            return Task.CompletedTask;
        }
    }
}
