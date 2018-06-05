namespace Azure.WebApi.Controllers
{
    using System.Net;
    using System.Web.Http;
    using Models;
    using AutoMapper;
    using Service.Interfaces;
    using App_Start;
    using Helpers;

    public class LoginController : BaseApiController
    {

        private readonly ISecretKeyProvider _secretKeyProvider;
        private readonly string _secretAuthKey;

        public LoginController(IMapper mapper, ISecretKeyProvider secretKeyProvider,
                               ILoggerService loggerService) : base(mapper, loggerService)
        {
            _secretKeyProvider = secretKeyProvider;
            _secretAuthKey = _secretKeyProvider.GetSecret(AzureKeys.AuthTokenKey);

        }

        [HttpPost]
        public IHttpActionResult Authenticate([FromBody] LoginRequest login)
        {
            _loggerService.LogInformation("start - Authenticate");
            if (string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Bad credentials");
            }

            var isUsernamePasswordValid = login.Password == "admin";

            if (!isUsernamePasswordValid)
            {
                var loginResponse = new LoginResponse { ResponseMsg = {StatusCode = HttpStatusCode.Unauthorized}};
                IHttpActionResult response = ResponseMessage(loginResponse.ResponseMsg);
                return response;
            }

            _loggerService.LogInformation($"Get token for user: {login.UserName}");

            string jwToken = AuthTokenGenerator.CreateToken(login.UserName, _secretAuthKey);

            _loggerService.LogInformation("end - Authenticate");

            return Ok(jwToken);
        }
    }
}
