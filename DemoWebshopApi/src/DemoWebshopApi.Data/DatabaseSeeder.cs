using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DemoWebshopApi.Data
{
    public class DatabaseSeeder
    {
        public static async Task Seed(IServiceProvider applicationServices)
        {
            using (IServiceScope serviceScope = applicationServices.CreateScope())
            {
                WebshopContext context = serviceScope.ServiceProvider.GetRequiredService<WebshopContext>();

                context.Database.SetCommandTimeout(600);
                if (context.Database.EnsureCreated())
                {
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<IIdentityUserManager>();

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

                    context.Roles.Add(adminRole);
                    context.Roles.Add(userRole);
                    context.SaveChanges();

                    await userManager.CreateUserAsync(admin, "adminpass");
                    await userManager.AddUserToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
