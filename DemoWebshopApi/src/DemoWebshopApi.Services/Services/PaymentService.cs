using DemoWebshopApi.Services.DTOs;
using DemoWebshopApi.Services.Interfaces;
using OnlinePayments.Sdk;
using OnlinePayments.Sdk.Domain;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace DemoWebshopApi.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IClient _paymentPlatformClient;
        private readonly IValidationService _validationService;

        public PaymentService(IClient paymentPlatformClient, IValidationService validationService)
        {
            _paymentPlatformClient = paymentPlatformClient;
            _validationService = validationService;
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
                    PaymentProductFilters = new PaymentProductFiltersHostedCheckout
                    {
                        Exclude = new PaymentProductFilter
                        {
                            Products = new List<int?> { 3, 840 }
                        },
                        RestrictTo = new PaymentProductFilter
                        {
                            Groups = new List<string> { "Cards" },
                            //Products = new List<int?> { 1 }
                        }
                    },
                    ReturnUrl = redirectUrl
                },
                CardPaymentMethodSpecificInput = new CardPaymentMethodSpecificInputBase()
                {
                    AuthorizationMode = "SALE"
                }
            };
            return await _paymentPlatformClient.WithNewMerchant(merchantId).HostedCheckout.CreateHostedCheckout(hostedCheckoutRequest);
        }

        public async Task<CreatedTokenResponse> CreateToken(CardData input, string merchantId)
        {
            var tokenRequest = new CreateTokenRequest
            {
                PaymentProductId = 1,
                Card = new TokenCardSpecificInput
                {
                    Data = new TokenData
                    {
                        Card = new Card
                        {
                            CardholderName = input.CardholderName,
                            CardNumber = input.CardNumber,
                            Cvv = input.CardCVV,
                            ExpiryDate = input.ExpiryDate
                        }
                    }
                }
            };

            return await _paymentPlatformClient
                            .WithNewMerchant(merchantId)
                            .Tokens
                            .CreateToken(tokenRequest);
        }

        public async Task<TokenResponse> GetToken(string tokenId, string merchantId)
        {
            return await _paymentPlatformClient
                            .WithNewMerchant(merchantId)
                            .Tokens
                            .GetToken(tokenId);
        }

        public async Task<CreatePaymentResponse> PayServerToServer(ServerToServerPaymentInput input, string merchantId)
        {
            var paymentRequest = new CreatePaymentRequest
            {
                CardPaymentMethodSpecificInput = new CardPaymentMethodSpecificInput
                {
                    PaymentProductId = 1, //PASS
                    SkipAuthentication = false, //PASS
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

            if (input.CardData != null)
            {
                paymentRequest.CardPaymentMethodSpecificInput.Card = new Card
                {
                    CardholderName = input.CardData.CardholderName,
                    CardNumber = input.CardData.CardNumber,
                    Cvv = input.CardData.CardCVV,
                    ExpiryDate = input.CardData.ExpiryDate
                };
            }
            else if (input.Token != null)
            {
                paymentRequest.CardPaymentMethodSpecificInput.Token = input.Token;
            }

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

        public async Task<PaymentResponse> GetDirectPaymentResult(string paymentId, string merchantId)
        {
            return await _paymentPlatformClient
                .WithNewMerchant(merchantId)
                .Payments
                .GetPayment(paymentId);
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

        public async Task<CaptureResponse> AddBulkPayment(string paymentId, string merchantId)
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

        public void AddBatchPayment(CardPaymentInput input, BasePaymentProviderConfig config)
        {
            var paymentLine = $"{(long)(input.PaymentData.OrderAmount * 100)};{input.PaymentData.Currency};Visa;{input.CardData.CardNumber};{input.CardData.ExpiryDate};{Guid.NewGuid()};;{input.CardData.CardholderName};;SAL;;;;;;;;;;;;;;;;;{input.CardData.CardCVV};";
            AddBatchLine(config, paymentLine);
        }

        public void AddSubscription(CardPaymentInput input, BasePaymentProviderConfig config)
        {
            var subId = Guid.NewGuid();
            var paymentLine = $"ADDSUBS;{input.CardData.CardholderName};{input.CardData.CardNumber};{input.CardData.ExpiryDate};VISA;{config.MerchantId};{subId};{(long)(input.PaymentData.OrderAmount * 100)};{input.PaymentData.Currency};m;1;1;1;{DateTime.UtcNow:dd-MM-yyyy};{DateTime.UtcNow.AddYears(1):dd-MM-yyyy};;;;example@email.com;;;";
            AddBatchLine(config, paymentLine);
        }

        private void AddBatchLine(BasePaymentProviderConfig config, string lineText)
        {
            var directoryName = "files";
            var fileName = $"paymentsBatch_{config.UserId}_{DateTime.UtcNow:yyyyMMdd_HHmm}.txt";
            var filePath = $"{directoryName}\\{fileName}";
            Directory.CreateDirectory(directoryName);

            var fileExists = File.Exists(filePath);
            using (var fs = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    // TODO: OrderId must received as input
                    var sb = new StringBuilder();
                    if (!fileExists)
                    {
                        sb.AppendLine($"OHL;{config.MerchantId};{config.MerchantPass};;{config.ApiUser};");
                        sb.AppendLine($"OHF;{fileName.Replace(".txt", $"")};ATR;SAL;1;");
                    }
                    else
                    {
                        var contentLines = sr.ReadToEnd().ToString().Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                        var ohfLine = contentLines[1];
                        var nb_payments = ohfLine.Substring(ohfLine.LastIndexOf(';', ohfLine.Length - 2) + 1);
                        contentLines[1] = ohfLine.Replace($"ATR;SAL;{nb_payments}", $"ATR;SAL;{contentLines.Length - 3 + 1}");
                        sb.AppendLine(string.Join("\r\n", contentLines));
                        sb.Remove(sb.Length - 6, 6);
                    }

                    sb.AppendLine(lineText);
                    sb.AppendLine($"OTF;");

                    var content = new UTF8Encoding(true).GetBytes(sb.ToString());
                    fs.SetLength(0);
                    fs.Write(content, 0, content.Length);
                }
            }
        }

        public async Task<IDictionary<string, List<string>>> ProcessBatchPayments(string batchEndpoint)
        {
            var processedFiles = new Dictionary<string, List<string>>();
            processedFiles.Add("successful", new List<string>());
            processedFiles.Add("failed", new List<string>());

            var directoryName = "files";
            var archiveDirectoryName = "archive";
            Directory.CreateDirectory(directoryName);
            Directory.CreateDirectory(archiveDirectoryName);
            var files = Directory.GetFiles(directoryName);
            _validationService.EnsureArrayNotEmpty(files, "Batch files");
            foreach (var filePath in files)
            {
                var fileContent = File.ReadAllText(filePath);
                var dict = new SortedDictionary<string, string>();
                dict.Add("FILE", fileContent);
                dict.Add("REPLY_TYPE", "XML");
                dict.Add("MODE", "SYNC");
                dict.Add("PROCESS_MODE", "CHECKANDPROCESS");
                var request = new HttpRequestMessage(HttpMethod.Post, batchEndpoint)
                {
                    Content = new FormUrlEncodedContent(dict)
                };

                var client = new HttpClient();
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string xml = await response.Content.ReadAsStringAsync();
                    var doc = XDocument.Parse(xml);
                    File.Move(filePath, filePath.Replace(directoryName, archiveDirectoryName));
                    processedFiles["successful"].Add(filePath.Replace($"{directoryName}\\", "").Replace(".txt", ""));
                }
                else
                {
                    processedFiles["failed"].Add(filePath.Replace($"{directoryName}\\", "").Replace(".txt", ""));
                }
            }

            return processedFiles;
        }

        public async Task<bool> AddScheduledPayment(CardPaymentInput input, ScheduledPaymentProviderConfig config, int paymentsCount = 3)
        {
            var dict = new SortedDictionary<string, string>();
            var monthlyPayment = (long)(input.PaymentData.OrderAmount / paymentsCount * 100);
            var paymentAmount = (long)(input.PaymentData.OrderAmount * 100);
            var paymentDate = DateTime.UtcNow;
            dict.Add("PSPID", config.MerchantId);
            // TODO: Replace fake with real OrderId
            dict.Add("ORDERID", Guid.NewGuid().ToString());
            dict.Add("USERID", config.ApiUser);
            dict.Add("PSWD", config.MerchantPass);
            dict.Add("CARDNO", input.CardData.CardNumber);
            dict.Add("ED", input.CardData.ExpiryDate);
            dict.Add("CVC", input.CardData.CardCVV);
            dict.Add("AMOUNT", paymentAmount.ToString());
            for (int i = 0; i < paymentsCount; i++)
            {
                if (i != paymentsCount - 1)
                {
                    dict.Add($"AMOUNT{i + 1}", monthlyPayment.ToString());
                }
                else
                {
                    dict.Add($"AMOUNT{i + 1}", (monthlyPayment + (paymentAmount - (paymentsCount * monthlyPayment))).ToString());
                }
                dict.Add($"EXECUTIONDATE{i + 1}", paymentDate.AddMonths(i).ToString("dd/MM/yyyy"));
            }
            dict.Add("CURRENCY", input.PaymentData.Currency);
            dict.Add("SHASIGN", CreateShaSign(dict, config.ShaKey));
            var request = new HttpRequestMessage(HttpMethod.Post, config.ScheduledPaymentEndpoint)
            {
                Content = new FormUrlEncodedContent(dict)
            };

            var client = new HttpClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string xml = await response.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(xml);
                if (doc.Root.Attribute("NCERROR")?.Value == "0" && doc.Root.Attribute("STATUS")?.Value == "56")
                {
                    return true;
                }
            }

            return false;
        }

        private static string CreateShaSign(SortedDictionary<string, string> requestFormParameters, string shaKey)
        {
            var sb = new StringBuilder();
            foreach(var parameterPair in requestFormParameters)
            {
                sb.Append($"{parameterPair.Key}={parameterPair.Value}{shaKey}");
            }

            return Hash(sb.ToString());
        }

        static string Hash(string input)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
