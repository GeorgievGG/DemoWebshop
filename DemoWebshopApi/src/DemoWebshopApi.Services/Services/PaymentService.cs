using DemoWebshopApi.Services.Interfaces;
using OnlinePayments.Sdk;
using OnlinePayments.Sdk.Domain;

namespace DemoWebshopApi.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IClient _paymentPlatformClient;

        public PaymentService(IClient paymentPlatformClient)
        {
            _paymentPlatformClient = paymentPlatformClient;
        }

        public async Task<CreateHostedCheckoutResponse> RequestHostedCheckoutPage(decimal paymentAmount, string redirectUrl, string merchantId)
        {
            var hostedCheckoutRequest = new CreateHostedCheckoutRequest
            {
                Order = new Order
                {
                    AmountOfMoney = new AmountOfMoney
                    {
                        Amount = (long)(paymentAmount * 100),
                        CurrencyCode = "EUR"
                    }
                },
                HostedCheckoutSpecificInput = new HostedCheckoutSpecificInput()
                {
                    ReturnUrl = redirectUrl
                },
                CardPaymentMethodSpecificInput = new CardPaymentMethodSpecificInputBase()
                {
                    AuthorizationMode = "SALE"
                }
            };
            return await _paymentPlatformClient.WithNewMerchant(merchantId).HostedCheckout.CreateHostedCheckout(hostedCheckoutRequest);
        }

        public async Task<GetHostedCheckoutResponse> GetHostedCheckoutPaymentResult(string hostedCheckoutId, string merchantId)
        {
            return await _paymentPlatformClient
                .WithNewMerchant(merchantId)
                .HostedCheckout
                .GetHostedCheckout(hostedCheckoutId);
        }
    }
}
