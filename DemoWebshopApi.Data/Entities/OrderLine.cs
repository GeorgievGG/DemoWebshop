namespace DemoWebshopApi.Data.Entities
{
    public class OrderLine
    {
        public Guid Id { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }

        public Guid OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
