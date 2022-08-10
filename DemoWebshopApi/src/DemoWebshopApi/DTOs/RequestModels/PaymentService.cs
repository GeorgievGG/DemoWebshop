//using Ingenico.Direct.Sdk;
//using Ingenico.Direct.Sdk.Domain;
//using MyWebShop.Checkout.DataAccess;
//using MyWebShop.Checkout.DataAccess.Entities;
//using MyWebShop.Checkout.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace MyWebShop.Checkout.Services
//{
//    public class PaymentService : IPaymentService
//    {
//        private readonly ICheckoutConfiguration _checkoutConfiguration;
//        private readonly CheckoutContext _checkoutContext;
//        private readonly GetPaymentProductsResponse paymentMethodsAvailable;

//        public PaymentService(ICheckoutConfiguration checkoutConfiguration, CheckoutContext checkoutContex)
//        {
//            _checkoutContext = checkoutContex;
//            _checkoutConfiguration = checkoutConfiguration;

//            paymentMethodsAvailable = CreateIngenicoDirectClient().WithNewMerchant(_checkoutConfiguration.MerchantId).Products
//                .GetPaymentProducts(new Ingenico.Direct.Sdk.Merchant.Products.GetPaymentProductsParams { CountryCode = _checkoutConfiguration.CountryCode, CurrencyCode = _checkoutConfiguration.Currency })
//                .GetAwaiter().GetResult();
//        }

//        public GetPaymentProductsResponse PaymentMethods
//        {
//            get
//            {
//                return paymentMethodsAvailable;
//            }
//        }

//        #region Payments


//        public async Task RefundPayment(string orderId, double amount)
//        {
//            var paymentDb = _checkoutContext.Transactions.FirstOrDefault(t => t.OrderId == Guid.Parse(orderId));
//            if (paymentDb != null)
//            {
//                var paymentId = paymentDb.PaymentId;
//                try
//                {
//                    RefundResponse response = await CreateIngenicoDirectClient()
//                                            .WithNewMerchant(_checkoutConfiguration.MerchantId)
//                                            .Payments
//                                            .RefundPayment(paymentId, BuildRefund(amount));

//                    var existingTransaction = _checkoutContext.Transactions.Where(x => x.PaymentId == paymentId).OrderByDescending(x => x.TransDate).FirstOrDefault();

//                    if (existingTransaction != null)
//                    {
//                        var paymentDetails = await GetPayment(paymentId);

//                        _checkoutContext.Transactions.Add(new Transactions
//                        {
//                            Amount = amount,
//                            MerchantRef = paymentDetails.PaymentOutput.References.MerchantReference,
//                            OrderId = existingTransaction.OrderId,
//                            Status = paymentDetails.Status,
//                            TransDate = DateTime.UtcNow,
//                            Type = paymentDetails.StatusOutput.StatusCategory,
//                            PaymentMethodId = existingTransaction.PaymentMethodId,
//                            PaymentId = paymentId
//                        });

//                        _checkoutContext.Transactions.Add(new Transactions
//                        {
//                            Amount = -amount,
//                            MerchantRef = response.RefundOutput.References.MerchantReference,
//                            OrderId = existingTransaction.OrderId,
//                            Status = response.Status,
//                            TransDate = DateTime.UtcNow,
//                            Type = "Refunding",
//                            PaymentMethodId = existingTransaction.PaymentMethodId,
//                            PaymentId = response.Id
//                        });

//                        await _checkoutContext.SaveChangesAsync();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    var x = ex;
//                }
//            }
//        }

//        public async Task CancelPayment(string orderId)
//        {
//            try
//            {
//                var paymentDb = _checkoutContext.Transactions.FirstOrDefault(t => t.OrderId == Guid.Parse(orderId));
//                if (paymentDb != null)
//                {
//                    var paymentId = paymentDb.PaymentId;
//                    CancelPaymentResponse response = await CreateIngenicoDirectClient()
//                                                     .WithNewMerchant(_checkoutConfiguration.MerchantId)
//                                                     .Payments
//                                                     .CancelPayment(paymentId);

//                    var existingTransaction = _checkoutContext.Transactions.Where(x => x.PaymentId == paymentId).OrderByDescending(x => x.TransDate).FirstOrDefault();
//                    if (existingTransaction != null)
//                    {
//                        var paymentDetails = await GetPayment(paymentId);

//                        _checkoutContext.Transactions.Add(new Transactions
//                        {
//                            Amount = existingTransaction.Amount,
//                            MerchantRef = paymentDetails.PaymentOutput.References.MerchantReference,
//                            OrderId = existingTransaction.OrderId,
//                            Status = paymentDetails.Status,
//                            TransDate = DateTime.UtcNow,
//                            Type = paymentDetails.StatusOutput.StatusCategory,
//                            PaymentMethodId = existingTransaction.PaymentMethodId,
//                            PaymentId = paymentId
//                        });

//                        _checkoutContext.Transactions.Add(new Transactions
//                        {
//                            Amount = -existingTransaction.Amount,
//                            MerchantRef = response.Payment.PaymentOutput.References.MerchantReference,
//                            OrderId = existingTransaction.OrderId,
//                            Status = response.Payment.Status,
//                            TransDate = DateTime.UtcNow,
//                            Type = "Canceling",
//                            PaymentMethodId = existingTransaction.PaymentMethodId,
//                            PaymentId = response.Payment.Id
//                        });

//                        await _checkoutContext.SaveChangesAsync();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                var x = ex;
//            }
//        }

//        public async Task GetPaymentDetails(string paymentId)
//        {
//            PaymentDetailsResponse response = await CreateIngenicoDirectClient()
//                .WithNewMerchant(_checkoutConfiguration.MerchantId)
//                .Payments
//                .GetPaymentDetails(paymentId);

//            //var storedDetails = dbContext.Transactions.Where(x => x.PaymentId == paymentId).OrderByDescending(x => x.TransDate).First();

//            //This is not working!!! The status we get is outdated
//            //if (response.Status != storedDetails.Status)
//            //{
//            //    dbContext.Transactions.Add(new Database.Entities.Transaction
//            //    {
//            //        Amount = (response.PaymentOutput.AmountOfMoney.Amount.HasValue) ? ((double)response.PaymentOutput.AmountOfMoney.Amount.Value) / 100 : storedDetails.Amount,
//            //        MerchantRef = response.PaymentOutput.References.MerchantReference,
//            //        OrderId = storedDetails.OrderId,
//            //        Status = response.Status,
//            //        TransDate = DateTime.UtcNow,
//            //        Type = response.StatusOutput.StatusCategory,
//            //        PaymentMethodId = storedDetails.PaymentMethodId,
//            //        PaymentId = response.Id
//            //    });

//            //    _ = await dbContext.SaveChangesAsync();
//            //}
//        }

//        #endregion

//        #region Tokens

//        public async Task<CreatedTokenResponse> CreateToken(CheckoutModel order)
//        {
//            return await CreateIngenicoDirectClient()
//                        .WithNewMerchant(_checkoutConfiguration.MerchantId)
//                        .Tokens
//                        .CreateToken(BuildCreateToken(order));
//        }

//        public async Task<TokenResponse> GetToken(string tokenId)
//        {
//            try
//            {
//                return await CreateIngenicoDirectClient()
//                        .WithNewMerchant(_checkoutConfiguration.MerchantId)
//                        .Tokens
//                        .GetToken(tokenId);
//            }
//            catch (Exception ex)
//            {
//                var x = ex;
//                throw new Exception(x.Message);
//            }

//        }

//        public async Task DeleteToken(string tokenId)
//        {
//            await CreateIngenicoDirectClient()
//                .WithNewMerchant(_checkoutConfiguration.MerchantId)
//                .Tokens
//                .DeleteToken(tokenId);
//        }

//        public async Task<IEnumerable<PersonTokens>> GetTokensByCustomerAsync(string customerId)
//        {
//            var customerTokens = _checkoutContext.PersonTokens.Where(pt => pt.CustomerId == customerId);
//            foreach (var token in customerTokens)
//            {
//                var res = await GetToken(token.Token);
//                token.Name = res.Card.Data.CardWithoutCvv.CardNumber;
//            }
//            return customerTokens;
//        }
           
//        #endregion

//        #region Private Helpers

//        private CreateTokenRequest BuildCreateToken(CheckoutModel order)
//        {
//            return new CreateTokenRequest
//            {
//                PaymentProductId = GetPaymentMethodId(), //order.PaymentMethodId,
//                Card = new TokenCardSpecificInput
//                {
//                    Data = new TokenData
//                    {
//                        Card = BuildCard(order.CardDetails)
//                    }
//                }
//            };
//        }

//        private CreatePaymentRequest BuildCC(CheckoutModel order, double amount, Guid orderId, string aliasId, bool isNewAlias, string authorizationMode)
//        {
//            var paymentMethodId = GetPaymentMethodId();

//            var request = new CreatePaymentRequest
//            {
//                CardPaymentMethodSpecificInput = new CardPaymentMethodSpecificInput
//                {
//                    AuthorizationMode = authorizationMode, //(selectedSale == SaleType.Sale) ? "SALE" : "FINAL_AUTHORIZATION",
//                    PaymentProductId = paymentMethodId, //order.PaymentMethodId,
//                    Card = BuildCard(order.CardDetails),
//                    Tokenize = isNewAlias, //order.CreateAlias,
//                    Token = aliasId// order.AliasId
//                },

//                Order = new Order
//                {
//                    AmountOfMoney = new AmountOfMoney { Amount = (long)(amount * 100), CurrencyCode = "EUR" },
//                    Customer = new Customer { BillingAddress = BuildAddress(order.ShippingDetails) },
//                    References = new OrderReferences { Descriptor = "", MerchantReference = orderId.ToString() },
//                    Shipping = new Shipping { Address = BuildAddressPersonal(order.ShippingDetails) }
//                }
//            };
//            return request;
//        }

//        private CreatePaymentRequest BuildCC(CheckoutModel order, double amount, Guid orderId, string aliasId, bool isNewAlias, bool isRecurring)
//        {
//            var paymentMethodId = GetPaymentMethodId();

//            var request = new CreatePaymentRequest
//            {
//                CardPaymentMethodSpecificInput = new CardPaymentMethodSpecificInput
//                {
//                    AuthorizationMode = "SALE", //(selectedSale == SaleType.Sale) ? "SALE" : "FINAL_AUTHORIZATION",
//                    PaymentProductId = paymentMethodId, //order.PaymentMethodId,
//                    Card = BuildCard(order.CardDetails),
//                    Tokenize = isNewAlias, //order.CreateAlias,
//                    Token = aliasId  // order.AliasId
//                },

//                Order = new Order
//                {
//                    AmountOfMoney = new AmountOfMoney { Amount = (long)(amount * 100), CurrencyCode = "EUR" },
//                    Customer = new Customer { BillingAddress = BuildAddress(order.ShippingDetails) },
//                    References = new OrderReferences { Descriptor = "", MerchantReference = orderId.ToString() },
//                    Shipping = new Shipping { Address = BuildAddressPersonal(order.ShippingDetails) }
//                }
//            };
//            return request;
//        }

//        private RefundRequest BuildRefund(double amount)
//        {
//            return new RefundRequest
//            {
//                AmountOfMoney = new AmountOfMoney { Amount = (long)(amount * 100), CurrencyCode = "EUR" },
//                References = new PaymentReferences { MerchantReference = _checkoutConfiguration.MerchantId }
//            };
//        }

//        private Address BuildAddress(ShippingDetailsModel address)
//        {
//            return new Address
//            {
//                City = address.City,
//                CountryCode = _checkoutConfiguration.CountryCode,
//                Street = address.Address,
//                Zip = address.ZipCode
//            };
//        }

//        private AddressPersonal BuildAddressPersonal(ShippingDetailsModel address)
//        {
//            return new AddressPersonal
//            {
//                City = address.City,
//                CountryCode = _checkoutConfiguration.CountryCode,
//                Street = address.Address,
//                Zip = address.ZipCode
//            };
//        }

//        private int GetPaymentMethodId()
//        {
//            var paymentMethods = paymentMethodsAvailable.PaymentProducts.FirstOrDefault();
//            return paymentMethods != null ? paymentMethods.Id.Value : 0;
//        }

//        #endregion
//    }
//}
