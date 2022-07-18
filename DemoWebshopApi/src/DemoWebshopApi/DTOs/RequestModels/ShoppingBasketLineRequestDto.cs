namespace DemoWebshopApi.DTOs.RequestModels
{
    public class ShoppingBasketLineRequestDto
    {
        public int Quantity { get; set; }

        public Guid ProductId { get; set; }
    }
}
