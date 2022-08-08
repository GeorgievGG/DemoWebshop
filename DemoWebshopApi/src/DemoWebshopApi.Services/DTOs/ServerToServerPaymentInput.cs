using System.ComponentModel.DataAnnotations;

namespace DemoWebshopApi.Services.DTOs
{
    public class ServerToServerPaymentInput
    {
        [Required]
        public CardData CardData { get; set; }
        [Required]
        public PaymentData PaymentData { get; set; }
        [Required]
        public BrowserData BrowserData { get; set; }
        [Required]
        public string RedirectUrl { get; set; }
    }
}