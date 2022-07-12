using DemoWebshopApi.DTOs.ResponseModels;

namespace DemoWebshopApi.DTOs.ResponseModels
{
    public class OrderLineResponseDto
    {
        public Guid Id { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public ProductResponseDto Product { get; set; }

        public Guid OrderId { get; set; }

    }
}
