using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Data.Interfaces;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Security.Claims;

namespace DemoWebshopApi.IdentityAuth
{
    public class PasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IIdentityUserManager _userManager;

        public PasswordValidator(IIdentityUserManager userManager)
        {
            _userManager = userManager;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {

           User user = await _userManager.FindByNameAsync(context.UserName);

            if (user != null)
            {
                bool authResult = await _userManager.ValidateUserCredentials(context.UserName, context.Password);

                if (authResult)
                {
                    List<string> roles = await _userManager.GetUserRolesAsync(user);
                    List<Claim> claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    context.Result = new GrantValidationResult(subject: user.Id.ToString(), authenticationMethod: "password", claims: claims);
                }
                else
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid login credentials");
                }

                return;
            }
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid login credentials");
        }
    }
}
