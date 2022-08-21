using AutoMapper;
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
        private readonly IMapper _mapper;

        public PaymentController(IOptions<OgoneSettings> paymentProviderSettings, IPaymentService paymentService, IMapper mapper)
        {
            _paymentProviderSettings = paymentProviderSettings;
            _paymentService = paymentService;
            _mapper = mapper;
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

        [HttpPost("Token")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> CreateToken(CardData input)
        {
            try
            {
                var createTokenResponse = await _paymentService.CreateToken(input, _paymentProviderSettings.Value.MerchantId);

                return Ok(createTokenResponse);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("Token/{tokenId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> GetToken(string tokenId)
        {
            try
            {
                var getTokenResponse = await _paymentService.GetToken(tokenId, _paymentProviderSettings.Value.MerchantId);

                return Ok(getTokenResponse);
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

        [HttpGet("{paymentId}/CheckDirectPaymentResult")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> CheckDirectPaymentResult(string paymentId)
        {
            try
            {
                var paymentResult = await _paymentService.GetDirectPaymentResult(paymentId, _paymentProviderSettings.Value.MerchantId);

                return Ok(paymentResult);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost("{paymentId}/CapturePayment")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> CapturePayment(string paymentId)
        {
            try
            {
                var paymentResponse = await _paymentService.CapturePayment(paymentId, _paymentProviderSettings.Value.MerchantId);

                return Ok(paymentResponse);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost("AddBatchPayment")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> AddBatchPayment(CardPaymentInput input)
        {
            try
            {
                var config = _mapper.Map<BasePaymentProviderConfig>(_paymentProviderSettings);
                config.UserId = UserId;

                _paymentService.AddBatchPayment(input, config);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("ProcessBatch")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ProcessBatchPayments()
        {
            try
            {
                var response = await _paymentService.ProcessBatchPayments(_paymentProviderSettings.Value.BatchEndpoint);

                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("AddScheduledPayment")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> AddScheduledPayment(CardPaymentInput input)
        {
            try
            {
                var config = _mapper.Map<ScheduledPaymentProviderConfig>(_paymentProviderSettings);
                config.UserId = UserId;

                var isSuccessful = await _paymentService.AddScheduledPayment(input, config);
                if (!isSuccessful)
                {
                    return BadRequest();
                }

                return Ok(isSuccessful);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("AddScheduled")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> AddSubscription(CardPaymentInput input)
        {
            try
            {
                var config = _mapper.Map<BasePaymentProviderConfig>(_paymentProviderSettings);
                config.UserId = UserId;

                _paymentService.AddSubscription(input, config);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}
