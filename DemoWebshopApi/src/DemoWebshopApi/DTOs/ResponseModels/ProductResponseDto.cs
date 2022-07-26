namespace DemoWebshopApi.DTOs.ResponseModels
{
    public class ProductResponseDto : ProductBaseResponseDto
    {

        public string PictureUrl { get; set; }

        public string Model { get; set; }
    }
}
