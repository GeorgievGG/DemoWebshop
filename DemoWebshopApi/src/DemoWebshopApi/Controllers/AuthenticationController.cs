using DemoWebshopApi.DTOs;
using DemoWebshopApi.DTOs.RequestModels;
using DemoWebshopApi.DTOs.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DemoWebshopApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IOptions<AuthSettings> _authSettings;

        public AuthenticationController(IOptions<AuthSettings> authSettings)
        {
            _authSettings = authSettings;
        }

        [HttpPost, Route("Login")]
        public async Task<ActionResult<OAuthTokenResponseDTO>> Login(AuthenticationLoginRequestDTO loginModel)
        {
            var client = new HttpClient();

            var url = "https://localhost:7000/connect/token";

            var identityServerParameters = new List<KeyValuePair<string, string>>();
            identityServerParameters.Add(new KeyValuePair<string, string>("grant_type", _authSettings.Value.GrantTypeLogin));
            identityServerParameters.Add(new KeyValuePair<string, string>("username", loginModel.Username));
            identityServerParameters.Add(new KeyValuePair<string, string>("password", loginModel.Password));
            identityServerParameters.Add(new KeyValuePair<string, string>("client_id", _authSettings.Value.ClientId));
            identityServerParameters.Add(new KeyValuePair<string, string>("client_secret", _authSettings.Value.ClientSecret));
            identityServerParameters.Add(new KeyValuePair<string, string>("scope", _authSettings.Value.Scope));

            using (client)
            {
                HttpResponseMessage response = await client.PostAsync(url, new FormUrlEncodedContent(identityServerParameters));
                var content = await response.Content.ReadAsStringAsync();
                var json = GetFormattedJson(content);

                return StatusCode((int)response.StatusCode, json);
            }
        }

        [HttpPost, Route("RefreshToken")]
        public async Task<ActionResult<OAuthTokenResponseDTO>> RefreshToken([FromBody]string refreshToken)
        {
            var client = new HttpClient();

            var url = "https://localhost:7000/connect/token";

            var identityServerParameters = new List<KeyValuePair<string, string>>();
            identityServerParameters.Add(new KeyValuePair<string, string>("grant_type", _authSettings.Value.GrantTypeRefresh));
            identityServerParameters.Add(new KeyValuePair<string, string>("client_id", _authSettings.Value.ClientId));
            identityServerParameters.Add(new KeyValuePair<string, string>("client_secret", _authSettings.Value.ClientSecret));
            identityServerParameters.Add(new KeyValuePair<string, string>("refresh_token", refreshToken));

            using (client)
            {
                HttpResponseMessage response = await client.PostAsync(url, new FormUrlEncodedContent(identityServerParameters));
                var content = await response.Content.ReadAsStringAsync();
                var json = GetFormattedJson(content);

                return StatusCode((int)response.StatusCode, json);
            }
        }

        private string GetFormattedJson(string content)
        {
            var jsonObject = JsonConvert.DeserializeObject<object>(content);
            return JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
        }
    }
}
