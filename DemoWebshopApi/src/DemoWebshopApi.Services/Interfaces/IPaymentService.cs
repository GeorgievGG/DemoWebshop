using DemoWebshopApi.Services.DTOs;
using OnlinePayments.Sdk.Domain;

namespace DemoWebshopApi.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<CreateHostedCheckoutResponse> RequestHostedCheckoutPage(decimal paymentAmount, string currency, string redirectUrl, string merchantId);
        Task<CreatePaymentResponse> PayServerToServer(ServerToServerPaymentInput input, string merchantId);
        Task<GetHostedCheckoutResponse> GetHostedCheckoutPaymentResult(string hostedCheckoutId, string merchantId);
        Task<CaptureResponse> CapturePayment(string paymentId, string merchantId);
    }
}