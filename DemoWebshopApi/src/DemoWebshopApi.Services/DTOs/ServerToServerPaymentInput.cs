using System.ComponentModel.DataAnnotations;

namespace DemoWebshopApi.Services.DTOs
{
    public class ServerToServerPaymentInput
    {
        public CardData? CardData { get; set; }
        public string? Token { get; set; }
        [Required]
        public PaymentData PaymentData { get; set; }
        [Required]
        public BrowserData BrowserData { get; set; }
        [Required]
        public string RedirectUrl { get; set; }
    }
}