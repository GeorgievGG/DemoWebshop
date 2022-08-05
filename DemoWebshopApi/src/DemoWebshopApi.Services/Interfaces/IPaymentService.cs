using OnlinePayments.Sdk.Domain;

namespace DemoWebshopApi.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<CreateHostedCheckoutResponse> RequestHostedCheckoutPage(decimal paymentAmount, string redirectUrl, string merchantId);
        Task<GetHostedCheckoutResponse> GetHostedCheckoutPaymentResult(string hostedCheckoutId, string merchantId);
    }
}