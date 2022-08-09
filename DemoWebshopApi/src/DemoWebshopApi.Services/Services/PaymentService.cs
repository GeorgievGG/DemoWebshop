using DemoWebshopApi.Services.DTOs;
using DemoWebshopApi.Services.Interfaces;
using OnlinePayments.Sdk;
using OnlinePayments.Sdk.Domain;
using OnlinePayments.Sdk.Merchant.Products;

namespace DemoWebshopApi.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IClient _paymentPlatformClient;

        public PaymentService(IClient paymentPlatformClient)
        {
            _paymentPlatformClient = paymentPlatformClient;
        }

        public async Task<CreateHostedCheckoutResponse> RequestHostedCheckoutPage(decimal paymentAmount, string currency, string redirectUrl, string merchantId)
        {
            var hostedCheckoutRequest = new CreateHostedCheckoutRequest
            {
                Order = new Order
                {
                    AmountOfMoney = new AmountOfMoney
                    {
                        Amount = (long)(paymentAmount * 100),
                        CurrencyCode = currency
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

        public async Task<CreatePaymentResponse> PayServerToServer(ServerToServerPaymentInput input, string merchantId)
        {
            var paymentRequest = new CreatePaymentRequest
            {
                CardPaymentMethodSpecificInput = new CardPaymentMethodSpecificInput
                {
                    PaymentProductId = 1,
                    SkipAuthentication = false,
                    Card = new Card
                    {
                        CardholderName = input.CardData.CardholderName,
                        CardNumber = input.CardData.CardNumber,
                        Cvv = input.CardData.CardCVV,
                        ExpiryDate = input.CardData.CardExpiryDate
                    },
                    ThreeDSecure = new ThreeDSecure
                    {
                        RedirectionData = new RedirectionData
                        {
                            ReturnUrl = input.RedirectUrl
                        }
                    }
                },
                Order = new Order
                {
                    AmountOfMoney = new AmountOfMoney
                    {
                        Amount = (long)(input.PaymentData.OrderAmount * 100),
                        CurrencyCode = input.PaymentData.Currency
                    },

                    Customer = new Customer
                    {
                        Device = new CustomerDevice
                        {
                            AcceptHeader = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3",
                            Locale = input.BrowserData.Locale,
                            TimezoneOffsetUtcMinutes = input.BrowserData.TimezoneOffsetUtcMinutes.ToString(),
                            UserAgent = input.BrowserData.UserAgent,
                            BrowserData = new OnlinePayments.Sdk.Domain.BrowserData
                            {
                                ColorDepth = input.BrowserData.ColorDepth,
                                JavaScriptEnabled = true,
                                ScreenHeight = input.BrowserData.ScreenHeight.ToString(),
                                ScreenWidth = input.BrowserData.ScreenWidth.ToString()
                            }
                        }
                    }
                }
            };

            return await _paymentPlatformClient
                            .WithNewMerchant(merchantId)
                            .Payments
                            .CreatePayment(paymentRequest);
        }

        public async Task<GetHostedCheckoutResponse> GetHostedCheckoutPaymentResult(string hostedCheckoutId, string merchantId)
        {
            return await _paymentPlatformClient
                .WithNewMerchant(merchantId)
                .HostedCheckout
                .GetHostedCheckout(hostedCheckoutId);
        }

        public async Task<CaptureResponse> CapturePayment(string paymentId, string merchantId)
        {
            var request = new CapturePaymentRequest
            {
                // INFO: I am aware how this amount works, but for simplicity I will only capture the full amount (default)
                // Amount = (long)(amount * 100),
                IsFinal = true,
                References = new PaymentReferences { MerchantReference = merchantId }
            };

            return await _paymentPlatformClient
                    .WithNewMerchant(merchantId)
                    .Payments
                    .CapturePayment(paymentId, request);
        }
    }
}
