using Blazored.LocalStorage;
using Blazored.Toast;
using FE_PRN232Project.Services;
using FE_PRN232Project.States;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.IdentityModel.Tokens.Jwt;

namespace WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5152/") });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore(options =>
            {
                // This is for development purposes only - allows accessing admin pages without authorization
                options.AddPolicy("AllowAnonymous", policy => policy.RequireAssertion(_ => true));
            });
            builder.Services.AddBlazoredToast();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            builder.Services.AddScoped<ApiAuthenticationStateProvider>(); // also register concrete class for calling Notify

            builder.Services.AddSingleton<CheckoutState>();

            builder.Services.AddHttpClient("AuthorizedAPI", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7041/");
            })
            .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            await builder.Build().RunAsync();
        }
    }
}
