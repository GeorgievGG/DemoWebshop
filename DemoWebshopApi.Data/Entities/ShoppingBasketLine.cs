namespace DemoWebshopApi.Data.Entities
{
    public class ShoppingBasketLine
    {
        public Guid Id { get; set; }

        public int Quantity { get; set; }

        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }

        public Guid BasketId { get; set; }

        public virtual ShoppingBasket Basket { get; set; }
    }
}
