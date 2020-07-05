using IdentityModel;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProvider
{
    public class ProfileWithRoleIdentityResource : IdentityResources.Profile
    {
        public ProfileWithRoleIdentityResource()
        {
            this.UserClaims.Add(JwtClaimTypes.Role);
        }
    }
}
