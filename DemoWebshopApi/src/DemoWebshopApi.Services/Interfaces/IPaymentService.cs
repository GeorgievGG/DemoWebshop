using OnlinePayments.Sdk.Domain;

namespace DemoWebshopApi.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<CreateHostedCheckoutResponse> RequestHostedCheckoutPage(decimal paymentAmount, string merchantId);
    }
}