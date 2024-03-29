﻿using Newtonsoft.Json;

namespace DemoWebshopApi.DTOs.ResponseModels
{
    public class OAuthTokenResponseDTO
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("scopes")]
        public string Scopes { get; set; }
    }
}
