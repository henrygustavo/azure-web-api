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

            var loginResponse = new LoginResponse();
            LoginRequest loginrequest = new LoginRequest
            {
                UsernNme = login.UsernNme.ToLower(),
                Password = login.Password
            };

            var isUsernamePasswordValid = loginrequest.Password == "admin";

            if (!isUsernamePasswordValid)
            {
                loginResponse.ResponseMsg.StatusCode = HttpStatusCode.Unauthorized;
                IHttpActionResult response = ResponseMessage(loginResponse.ResponseMsg);
                return response;
            }


            _loggerService.LogInformation($"Get token for user: {login.UsernNme}");

            string jwToken = AuthTokenGenerator.CreateToken(loginrequest.UsernNme, _secretAuthKey);

            _loggerService.LogInformation("end - Authenticate");

            return Ok(jwToken);
        }
    }
}
