using System.ComponentModel.DataAnnotations;

namespace DemoWebshopApi.Services.DTOs
{
    public class BrowserData
    {
        [Required]
        public string Locale { get; set; }
        [Required]
        public int TimezoneOffsetUtcMinutes { get; set; }
        [Required]
        public string UserAgent { get; set; }
        [Required]
        public int ColorDepth { get; set; }
        [Required]
        public int ScreenHeight { get; set; }
        [Required]
        public int ScreenWidth { get; set; }
    }
}