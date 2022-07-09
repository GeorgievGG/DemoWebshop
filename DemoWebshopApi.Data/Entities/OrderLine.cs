namespace DemoWebshopApi.Data.Entities
{
    public class OrderLine
    {
        public Guid Id { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}
