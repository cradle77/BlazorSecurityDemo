using BlazorClient.Security;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddHttpClient("api")
                .AddHttpMessageHandler(sp =>
                {
                    var handler = sp.GetService<AuthorizationMessageHandler>()
                        .ConfigureHandler(
                            authorizedUrls: new[] { "https://localhost:5002" },
                            scopes: new[] { "weatherapi" });

                    return handler;
                });

            builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("api"));

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<RemoteAuthenticationOptions<ExtendedOidcProviderOptions>>, DefaultOidcOptionsConfiguration<ExtendedOidcProviderOptions>>());

            builder.Services.AddRemoteAuthentication<RemoteAuthenticationState, RemoteUserAccount, ExtendedOidcProviderOptions>(options =>
            {
                builder.Configuration.Bind("oidc", options.ProviderOptions);
                options.UserOptions.RoleClaim = "role";
                options.ProviderOptions.AcrValues = "tenant:mytenant";
            })
                .AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

            await builder.Build().RunAsync();
        }
    }
}
