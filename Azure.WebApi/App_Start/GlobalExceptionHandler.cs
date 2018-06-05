namespace Azure.WebApi.App_Start
{
    using Service.Interfaces;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.ExceptionHandling;
    using System.Web.Http.Results;

    public class GlobalExceptionHandler : ExceptionHandler
    {
        protected readonly ILoggerService _loggerService;

        public GlobalExceptionHandler(ILoggerService loggerService)
        {
            _loggerService = loggerService;

        }
        public override void Handle(ExceptionHandlerContext context)
        {
            _loggerService.LogError($"message: {context.Exception.Message}, stackTrace: {context.Exception.StackTrace}");

            // Access Exception using context.Exception;  
            const string errorMessage = "An unexpected error occured";
            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
                new
                {
                    Message = errorMessage
                });
            response.Headers.Add("X-Error", errorMessage);
            context.Result = new ResponseMessageResult(response);
        }
    }
}