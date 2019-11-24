using Identity.API.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.API.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentException(nameof(context.Subject));
            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
            var user = await userManager.FindByIdAsync(subjectId);
            if (user == null)
                throw new ArgumentException("invalid subject Identifier");

            var claims = GetClaimsFromUser(user);
            context.IssuedClaims = claims.ToList();

        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Claim> GetClaimsFromUser(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject,user.Id),
                new Claim(JwtClaimTypes.PreferredUserName,user.UserName),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.Name),

            };
            if (!string.IsNullOrWhiteSpace(user.Name))
                claims.Add(new Claim("name",user.Name));
            if (!string.IsNullOrWhiteSpace(user.Name))
                claims.Add(new Claim("last_name", user.LastName));
            if (!string.IsNullOrWhiteSpace(user.Name))
                claims.Add(new Claim("address_city", user.City));
            if (!string.IsNullOrWhiteSpace(user.Name))
                claims.Add(new Claim("address_country", user.Country));
            if (!string.IsNullOrWhiteSpace(user.Name))
                claims.Add(new Claim("address_state", user.State));
            if (!string.IsNullOrWhiteSpace(user.Name))
                claims.Add(new Claim("address_street", user.ZipCode));
            if (!string.IsNullOrWhiteSpace(user.Name))
                claims.Add(new Claim("address_zip_code", user.Name));

            if(userManager.SupportsUserEmail)
            {
                claims.AddRange(new[] {
                    new Claim(JwtClaimTypes.Email,user.Email),
                    new Claim(JwtClaimTypes.EmailVerified,user.EmailConfirmed ? "true":"false",ClaimValueTypes.Boolean)
                });
            }

            if (userManager.SupportsUserPhoneNumber)
            {
                claims.AddRange(new[] {
                    new Claim(JwtClaimTypes.PhoneNumber,user.PhoneNumber),
                    new Claim(JwtClaimTypes.PhoneNumberVerified,user.PhoneNumberConfirmed ? "true":"false",ClaimValueTypes.Boolean)
                });
            }

            return claims;
        }
    }
}
