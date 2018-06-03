namespace Azure.WebApi.Controllers
{
    using System.Web.Http;
    using Service.Interfaces;
    using AutoMapper;

    public abstract class BaseController : ApiController
    {
        protected readonly IMapper _mapper;
        protected readonly ILoggerService _loggerService;

        protected BaseController(IMapper mapper, ILoggerService loggerService)
        {
            _mapper = mapper;
            _loggerService = loggerService;
        }
    }
}
