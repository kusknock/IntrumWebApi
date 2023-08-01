﻿namespace IntrumWebApi.Models
{
    public class TokenModel
    {
        public string? AccessToken { get; set; } = null!;
        public string? RefreshToken { get; set; } = null!;

        public TokenModel(string? accessToken, string? refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
        public TokenModel() { }
    }
}
