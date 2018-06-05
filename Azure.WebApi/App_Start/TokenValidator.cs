namespace Azure.WebApi.App_Start
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.IdentityModel.Tokens;
    using Service.Interfaces;
    using Microsoft.Azure;

    internal class TokenValidationHandler : DelegatingHandler
    {
        private readonly ISecretKeyProvider _secretKeyProvider;
        private readonly string _secretAuthKey;

        public TokenValidationHandler(ISecretKeyProvider secretKeyProvider)
        {
            _secretKeyProvider = secretKeyProvider;
            _secretAuthKey = _secretKeyProvider.GetSecret(AzureKeys.AuthTokenKey);

        }
        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            if (!request.Headers.TryGetValues("Authorization", out var authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!TryRetrieveToken(request, out var token))
            {
                return base.SendAsync(request, cancellationToken);
            }

            HttpStatusCode statusCode;
            try
            {
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(_secretAuthKey));

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                string baseUrl = CloudConfigurationManager.GetSetting("BaseUrl");

                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = baseUrl,
                    ValidIssuer = baseUrl,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = LifetimeValidator,
                    IssuerSigningKey = securityKey
                };

                Thread.CurrentPrincipal = handler.ValidateToken(token, validationParameters, out _);
                HttpContext.Current.User = handler.ValidateToken(token, validationParameters, out _);

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException e)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception ex)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }
            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode), cancellationToken);
        }

        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires,
                                      SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires == null) return false;
            return DateTime.UtcNow < expires;
        }
    }
}