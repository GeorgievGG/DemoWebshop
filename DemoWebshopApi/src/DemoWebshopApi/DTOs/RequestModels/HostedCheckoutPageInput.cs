using System.ComponentModel.DataAnnotations;

namespace DemoWebshopApi.DTOs.RequestModels
{
    public class HostedCheckoutPageInput
    {
        [Required]
        public decimal OrderAmount { get; set; }
    }
}
