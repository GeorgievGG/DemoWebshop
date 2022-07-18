namespace DemoWebshopApi.Data.Entities
{
    public class ShoppingBasket
    {
        public ShoppingBasket()
        {
            BasketLines = new List<ShoppingBasketLine>();
        }

        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public virtual User Client { get; set; }

        public virtual ICollection<ShoppingBasketLine> BasketLines { get; set; }
    }
}
