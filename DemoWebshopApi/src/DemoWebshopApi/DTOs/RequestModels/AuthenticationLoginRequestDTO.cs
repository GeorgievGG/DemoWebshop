using System.ComponentModel.DataAnnotations;

namespace DemoWebshopApi.DTOs.RequestModels
{
    public class AuthenticationLoginRequestDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
