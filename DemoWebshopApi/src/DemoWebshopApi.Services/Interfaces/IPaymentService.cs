using DemoWebshopApi.Services.DTOs;
using OnlinePayments.Sdk.Domain;

namespace DemoWebshopApi.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<CreateHostedCheckoutResponse> RequestHostedCheckoutPage(decimal paymentAmount, string currency, string redirectUrl, string merchantId);
        Task<CreatePaymentResponse> PayServerToServer(ServerToServerPaymentInput input, string merchantId);
        Task<CreatedTokenResponse> CreateToken(CardData input, string merchantId);
        Task<TokenResponse> GetToken(string tokenId, string merchantId);
        Task<GetHostedCheckoutResponse> GetHostedCheckoutPaymentResult(string hostedCheckoutId, string merchantId);
        Task<PaymentResponse> GetDirectPaymentResult(string paymentId, string merchantId);
        Task<CaptureResponse> CapturePayment(string paymentId, string merchantId);
        void AddBatchPayment(CardPaymentInput input, BasePaymentProviderConfig config);
        void AddSubscription(CardPaymentInput input, BasePaymentProviderConfig config);
        Task<IDictionary<string, List<string>>> ProcessBatchPayments(string batchEndpoint);
        Task<bool> AddScheduledPayment(CardPaymentInput input, ScheduledPaymentProviderConfig config, int paymentsCount = 3);
    }
}