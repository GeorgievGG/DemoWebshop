using DemoWebshopApi.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DemoWebshopApi.Data
{
    public class DatabaseSeeder
    {
        public static void Seed(IServiceProvider applicationServices)
        {
            using (IServiceScope serviceScope = applicationServices.CreateScope())
            {
                WebshopContext context = serviceScope.ServiceProvider.GetRequiredService<WebshopContext>();
                if (context.Database.EnsureCreated())
                {
                    PasswordHasher<User> hasher = new PasswordHasher<User>();

                    User admin = new User()
                    {
                        Id = Guid.NewGuid(),
                        UserName = "admin",
                        FirstName = "Admin",
                        LastName = "Admin",
                        Email = "admin@identity.com",
                        NormalizedEmail = "admin@identity.com".ToUpper(),
                        EmailConfirmed = true,
                        NormalizedUserName = "admin@identity.com".ToUpper(),
                        SecurityStamp = Guid.NewGuid().ToString("D"),

                    };

                    admin.PasswordHash = hasher.HashPassword(admin, "adminpass");

                    var adminRole = new ApplicationRole()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Admin",
                        NormalizedName = "Admin".ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString("D")
                    };

                    var userRole = new ApplicationRole()
                    {
                        Id = Guid.NewGuid(),
                        Name = "User",
                        NormalizedName = "User".ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString("D")
                    };


                    var initialAdminRole = new IdentityUserRole<Guid>()
                    {
                        RoleId = adminRole.Id,
                        UserId = admin.Id
                    };

                    context.Users.Add(admin);
                    context.Roles.Add(adminRole);
                    context.Roles.Add(userRole);
                    context.UserRoles.Add(initialAdminRole);
                    context.SaveChanges();

                }
            }
        }
    }
}
