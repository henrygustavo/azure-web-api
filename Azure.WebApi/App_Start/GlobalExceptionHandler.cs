namespace Azure.WebApi.App_Start
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.ExceptionHandling;
    using System.Web.Http.Results;

    public class GlobalExceptionHandler : ExceptionHandler
    {

        public override void Handle(ExceptionHandlerContext context)
        {
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