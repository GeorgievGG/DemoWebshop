namespace DemoWebshopApi.DTOs.RequestModels
{
    public class OrderLineRequestDto
    {
        public int Quantity { get; set; }

        public double Price { get; set; }

        public Guid ProductId { get; set; }
    }
}
