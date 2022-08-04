using DemoWebshopApi.DTOs;
using DemoWebshopApi.DTOs.RequestModels;
using DemoWebshopApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnlinePayments.Sdk;
using OnlinePayments.Sdk.Domain;

namespace DemoWebshopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseController
    {
        private readonly IOptions<OgoneSettings> _paymentProviderSettings;
        private readonly IPaymentService _paymentService;

        public PaymentController(IOptions<OgoneSettings> paymentProviderSettings, IPaymentService paymentService)
        {
            _paymentProviderSettings = paymentProviderSettings;
            _paymentService = paymentService;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> RequestHostedCheckoutPage(HostedCheckoutPageInput input)
        {
            try
            {
                var hostedCheckoutResponse = await _paymentService.RequestHostedCheckoutPage(input.OrderAmount, _paymentProviderSettings.Value.MerchantId); ;

                return Ok(hostedCheckoutResponse);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
