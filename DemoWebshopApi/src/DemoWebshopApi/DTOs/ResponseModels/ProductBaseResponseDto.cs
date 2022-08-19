namespace DemoWebshopApi.DTOs.ResponseModels
{
    public class ProductBaseResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public double Price { get; set; }

        public int AvailableQuantity { get; set; }

        public bool IsSubscription { get; set; }
    }
}