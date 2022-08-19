namespace DemoWebshopApi.Data.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string PictureUrl { get; set; }

        public string Name { get; set; }

        public string Model { get; set; }

        public int AvailableQuantity { get; set; }

        public double Price { get; set; }

        public bool IsSubscription { get; set; }
    }
}
