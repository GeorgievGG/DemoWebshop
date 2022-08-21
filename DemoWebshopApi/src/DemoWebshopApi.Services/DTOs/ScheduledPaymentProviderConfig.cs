namespace DemoWebshopApi.Services.DTOs
{
    public class ScheduledPaymentProviderConfig : BasePaymentProviderConfig
    {
        public string ScheduledPaymentEndpoint { get; set; }
        public string ShaKey { get; set; }
    }
}
