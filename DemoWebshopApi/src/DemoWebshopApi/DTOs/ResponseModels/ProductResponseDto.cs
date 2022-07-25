namespace DemoWebshopApi.DTOs.ResponseModels
{
    public class ProductResponseDto : ProductBaseResponseDto
    {
        public Guid Id { get; set; }

        public string PictureUrl { get; set; }

        public string Model { get; set; }
    }
}
