using DemoWebshopApi.DTOs;
using DemoWebshopApi.DTOs.RequestModels;
using DemoWebshopApi.Services.DTOs;
using DemoWebshopApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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

        [HttpPost("GetHostedCheckoutPage")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> RequestHostedCheckoutPage(HostedCheckoutPageInput input)
        {
            try
            {
                var hostedCheckoutResponse = await _paymentService.RequestHostedCheckoutPage(input.OrderAmount, input.Currency, input.RedirectUrl, _paymentProviderSettings.Value.MerchantId);

                return Ok(hostedCheckoutResponse);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost("ServerToServerPayment")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> PayServerToServer(ServerToServerPaymentInput input)
        {
            try
            {
                var paymentResponse = await _paymentService.PayServerToServer(input, _paymentProviderSettings.Value.MerchantId);

                return Ok(paymentResponse);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("{hostedCheckoutId}/CheckHostedCheckoutPagePaymentResult")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> CheckHostedCheckoutPagePaymentResult(string hostedCheckoutId)
        {
            try
            {
                var paymentResult = await _paymentService.GetHostedCheckoutPaymentResult(hostedCheckoutId, _paymentProviderSettings.Value.MerchantId);

                return Ok(paymentResult);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
