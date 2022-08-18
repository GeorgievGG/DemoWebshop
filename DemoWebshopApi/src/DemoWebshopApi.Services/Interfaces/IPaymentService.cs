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
        void AddBatchPayment(CardPaymentInput input, string merchantId, string userId, string ohlPassword, string apiUser);
        Task<IDictionary<string, List<string>>> ProcessBatchPayments(string batchEndpoint);
        Task<bool> AddScheduledPayment(CardPaymentInput input, string scheduledPaymentEndpoint, string merchantId, string password, string apiUser, string shaKey, int paymentsCount = 3);
    }
}