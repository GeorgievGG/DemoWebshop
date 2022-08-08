using System.ComponentModel.DataAnnotations;

namespace DemoWebshopApi.Services.DTOs
{
    public class CardData
    {
        [Required]
        public string CardholderName { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string CardCVV { get; set; }
        [Required]
        public string CardExpiryDate { get; set; }
    }
}