namespace DemoWebshopApi.DTOs.RequestModels
{
    public class ProductRequestDto
    {
        public string Name { get; set; }
        public string PictureUrl { get; set; }

        public string Model { get; set; }

        public int AvailableQuantity { get; set; }

        public double Price { get; set; }
    }
}
