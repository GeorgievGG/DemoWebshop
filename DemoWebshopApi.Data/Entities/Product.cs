namespace DemoWebshopApi.Data.Entities
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Model { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}
