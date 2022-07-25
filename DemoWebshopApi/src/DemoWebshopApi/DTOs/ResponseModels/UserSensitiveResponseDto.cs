namespace DemoWebshopApi.DTOs.ResponseModels
{
    public class UserSensitiveResponseDto : UserResponseDto
    {
        public bool IsAdmin { get; set; }
    }
}
