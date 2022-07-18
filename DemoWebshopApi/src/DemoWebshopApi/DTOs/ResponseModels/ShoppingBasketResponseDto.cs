namespace DemoWebshopApi.DTOs.ResponseModels
{
    public class ShoppingBasketResponseDto
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public virtual ICollection<ShoppingBasketLineResponseDto> BasketLines { get; set; }
    }
}
