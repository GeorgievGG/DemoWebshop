using System.ComponentModel.DataAnnotations;

namespace DemoWebshopApi.Services.DTOs
{
    public class CardPaymentInput
    {
        [Required]
        public CardData CardData { get; set; }
        [Required]
        public PaymentData PaymentData { get; set; }
    }
}
