namespace DemoWebshopApi.DTOs
{
    public class AuthSettings
    {
        public string GrantTypeLogin { get; set; }
        public string GrantTypeRefresh { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}
