using Microsoft.AspNetCore.Identity;

namespace DemoWebshopApi.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime LastLogin { get; set; }

        public virtual ShoppingBasket Basket { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
