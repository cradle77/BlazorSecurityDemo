using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorClient.Security
{
    public class RolesClaimsFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        public RolesClaimsFactory(IAccessTokenProviderAccessor accessor)
        : base(accessor)
        { }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var user = await base.CreateUserAsync(account, options);

            var claimsIdentity = (ClaimsIdentity)user.Identity;

            var roleClaim = claimsIdentity.FindFirst(claimsIdentity.RoleClaimType);

            string[] roles = null;

            try
            {
                roles = JsonSerializer.Deserialize<string[]>(roleClaim?.Value);
            }
            catch
            { }

            if (roleClaim != null && roles != null && roles.Any())
            {
                claimsIdentity.RemoveClaim(roleClaim);

                foreach (var role in roles)
                {
                    claimsIdentity.AddClaim(new Claim(claimsIdentity.RoleClaimType, role));
                }
            }

            return user;
        }
    }
}
