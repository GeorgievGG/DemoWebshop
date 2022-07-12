namespace DemoWebshopApi.DTOs.ResponseModels
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Model { get; set; }

        public int AvailableQuantity { get; set; }

        public double Price { get; set; }
    }
}
