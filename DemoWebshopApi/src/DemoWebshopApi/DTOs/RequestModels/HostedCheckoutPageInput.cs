﻿using System.ComponentModel.DataAnnotations;

namespace DemoWebshopApi.DTOs.RequestModels
{
    public class HostedCheckoutPageInput
    {
        [Required]
        public decimal OrderAmount { get; set; }
        [Required]
        public string RedirectUrl { get; set; }
        [Required]
        public string Currency { get; set; }
    }
}
