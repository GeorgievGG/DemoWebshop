﻿namespace DemoWebshopApi.DTOs.ResponseModels
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public bool Confirmed { get; set; }

        public DateTime DeliveryDate { get; set; }

        public Guid ClientId { get; set; }

        public ICollection<OrderLineResponseDto> OrderLines { get; set; }
    }
}
