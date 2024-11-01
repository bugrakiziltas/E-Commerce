using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServerAspNetIdentity
{
    public class Profile : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
    
        public Profile(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            var existingClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim("username", user.UserName)
            };
            claims.AddRange(existingClaims.Where(x=>x.Type==JwtClaimTypes.Name||x.Type==JwtClaimTypes.Role||x.Type==JwtClaimTypes.Id||x.Type==JwtClaimTypes.Email).ToList());
            context.IssuedClaims.AddRange(claims);
        }
        

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            context.IsActive = user != null;
        }
    }
}