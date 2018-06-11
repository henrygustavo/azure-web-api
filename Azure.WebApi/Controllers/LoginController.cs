namespace Azure.WebApi.Controllers
{
    using System.Net;
    using System.Web.Http;
    using Models;
    using AutoMapper;
    using Service.Interfaces;
    using App_Start;
    using Helpers;
    using Repository.Interfaces;

    public class LoginController : BaseApiController
    {

        private readonly ISecretKeyProvider _secretKeyProvider;
        private readonly string _secretAuthKey;
        private readonly IUserRepository _userRepository;

        public LoginController(IMapper mapper, ISecretKeyProvider secretKeyProvider,
                               ILoggerService loggerService, IUserRepository userRepository) : base(mapper, loggerService)
        {
            _secretKeyProvider = secretKeyProvider;
            _secretAuthKey = _secretKeyProvider.GetSecret(AzureKeys.AuthTokenKey);
            _userRepository = userRepository;

        }

        [HttpPost]
        public IHttpActionResult Authenticate([FromBody] LoginRequest login)
        {
            _loggerService.LogInformation("start - Authenticate");
            if (string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Bad credentials");
            }
            
            if (!_userRepository.HasValidCredentials(login.UserName, login.Password))
            {
                var loginResponse = new LoginResponse { ResponseMsg = {StatusCode = HttpStatusCode.Unauthorized}};
                IHttpActionResult response = ResponseMessage(loginResponse.ResponseMsg);
                return response;
            }

            _loggerService.LogInformation($"Get token for user: {login.UserName}");

            string jwToken = AuthTokenGenerator.CreateToken(login.UserName, _secretAuthKey);

            _loggerService.LogInformation("end - Authenticate");

            return Ok(new { token = jwToken});
        }
    }
}
