namespace DemoWebshopApi.DTOs.ResponseModels
{
    public class ShoppingBasketDetailedResponseDto
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public virtual ICollection<ShoppingBasketDetailedLineResponseDto> BasketLines { get; set; }
    }
}
