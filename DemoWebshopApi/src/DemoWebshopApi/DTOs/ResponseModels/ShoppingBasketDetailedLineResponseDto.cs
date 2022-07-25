namespace DemoWebshopApi.DTOs.ResponseModels
{
    public class ShoppingBasketDetailedLineResponseDto
    {
        public int Quantity { get; set; }

        public ProductBaseResponseDto Product { get; set; }
    }
}