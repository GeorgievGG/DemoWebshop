namespace DemoWebshopApi.DTOs.RequestModels
{
    public class OrderRequestDto
    {
        public ICollection<OrderLineRequestDto> OrderLines { get; set; }
    }
}
