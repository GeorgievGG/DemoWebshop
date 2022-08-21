namespace DemoWebshopApi.Services.DTOs
{
    public class BasePaymentProviderConfig
    {
        public string MerchantId { get; set; }
        public string MerchantPass { get; set; }
        public string UserId { get; set; }
        public string ApiUser { get; set; }
    }
}