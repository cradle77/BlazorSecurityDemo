using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Text.Json.Serialization;

namespace BlazorClient
{
    public class ExtendedOidcProviderOptions : OidcProviderOptions
    {
        [JsonPropertyName("acr_values")]
        public string AcrValues { get; set; }
    }
}
