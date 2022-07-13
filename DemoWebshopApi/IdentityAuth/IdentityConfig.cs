using IdentityServer4.Models;
using System.Security.Claims;

namespace DemoWebshopApi.IdentityAuth
{
    public class IdentityConfig
    {
        public static IEnumerable<Client> Clients =>
        new List<Client>
        {
           new Client
                 {
            ClientId = "DemoWebshopApi",
            AllowOfflineAccess = true,

            // no interactive user, use the clientid/secret for authentication
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

            // secret for authentication
            ClientSecrets =
            {
                new Secret("out_of_rangers_bros".Sha256())
            },

            // scopes that client has access to
            AllowedScopes = { "users", "offline_access", "DemoWebshopApi", "roles" }
                }

        };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResource("roles", new[] { "role" })
            };

        public static IEnumerable<ApiScope> ApiScopes =>
          new List<ApiScope>
          {
                new ApiScope("users", "My API", new string[]{ ClaimTypes.NameIdentifier, ClaimTypes.Name, ClaimTypes.Role }),
                new ApiScope("offline_access", "RefereshToken"),
                new ApiScope("DemoWebshopApi", "app")
          };
    }
}
