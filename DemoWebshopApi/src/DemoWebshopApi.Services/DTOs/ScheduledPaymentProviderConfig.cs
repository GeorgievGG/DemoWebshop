namespace DemoWebshopApi.Services.DTOs
{
    public class ScheduledPaymentProviderConfig : BasePaymentProviderConfig
    {
        public string OldschoolDirectPaymentEndpoint { get; set; }
        public string ShaKey { get; set; }
    }
}
