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

        public async Task<CreateHostedCheckoutResponse> RequestHostedCheckoutPage(decimal paymentAmount, string merchantId)
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
                }
            };

            return await _paymentPlatformClient.WithNewMerchant(merchantId).HostedCheckout.CreateHostedCheckout(hostedCheckoutRequest);
        }
    }
}
