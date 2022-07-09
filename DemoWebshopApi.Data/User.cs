using Microsoft.AspNetCore.Identity;

namespace DemoWebshopApi.Data
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
