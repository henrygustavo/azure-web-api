namespace Azure.WebApi.Helpers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using Microsoft.Azure;

    public static class AuthTokenGenerator
    {
        public static string CreateToken(string userName, string secretKey)
        {
            DateTime issuedAt = DateTime.UtcNow;

            DateTime expires = DateTime.UtcNow.AddDays(7);

            var tokenHandler = new JwtSecurityTokenHandler();

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName)
            });

            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

            string baseUrl = CloudConfigurationManager.GetSetting("BaseUrl");

            var jwToken =
                    tokenHandler.CreateJwtSecurityToken(issuer: baseUrl, audience: baseUrl,
                    subject: claimsIdentity, notBefore: issuedAt, expires: expires,
                    signingCredentials: signingCredentials);

            var tokenString = tokenHandler.WriteToken(jwToken);

            return tokenString;
        }
    }
}