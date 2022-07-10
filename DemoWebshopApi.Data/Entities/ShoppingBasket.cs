namespace DemoWebshopApi.Data.Entities
{
    public class ShoppingBasket
    {
        public Guid Id { get; set; }

        public DateTime LastVisited { get; set; }

        public Guid ClientId { get; set; }

        public virtual User Client { get; set; }
    }
}
