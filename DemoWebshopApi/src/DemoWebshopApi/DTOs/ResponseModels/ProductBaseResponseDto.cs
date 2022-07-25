namespace DemoWebshopApi.DTOs.ResponseModels
{
    public class ProductBaseResponseDto
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public int AvailableQuantity { get; set; }
    }
}