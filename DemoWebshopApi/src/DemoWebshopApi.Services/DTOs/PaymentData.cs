using System.ComponentModel.DataAnnotations;

namespace DemoWebshopApi.Services.DTOs
{
    public class PaymentData
    {
        [Required]
        public decimal OrderAmount { get; set; }
        [Required]
        public string Currency { get; set; }
    }
}