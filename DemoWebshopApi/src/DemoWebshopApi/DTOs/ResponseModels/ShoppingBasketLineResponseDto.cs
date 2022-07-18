namespace DemoWebshopApi.DTOs.ResponseModels
{
    public class ShoppingBasketLineResponseDto
    {
        public int Quantity { get; set; }

        public Guid ProductId { get; set; }
    }
}