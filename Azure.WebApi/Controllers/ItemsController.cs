namespace Azure.WebApi.Controllers
{
    using Service.Interfaces;
    using System.Web.Http;
    using Swashbuckle.Swagger.Annotations;
    using Repository.Interfaces;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using AutoMapper;
    using Models;
    using Entity;

    public class ItemsController : BaseController
    {
        private readonly IItemRepository _itemRepository;
        public ItemsController(IItemRepository itemRepository,
                               IMapper mapper,
                               ILoggerService loggerService) : base(mapper, loggerService)
        {
            _itemRepository = itemRepository;
        }
        // GET api/items
        [SwaggerOperation("GetAll")]
        public async Task<IHttpActionResult> Get()
        {

            _loggerService.LogInformation("start - GetAll Items");

            var entities = await _itemRepository.GetAllAsync();

             _loggerService.LogInformation("end -  GetAll Items");

            return Ok(_mapper.Map<List<ItemModel>>(entities));
        }

        //POST api/items
        [SwaggerOperation("Create")]
        public async Task<IHttpActionResult> Post(ItemModel entityModel)
        {
            _loggerService.LogInformation("start - Create Item");

            var newEntity =  await _itemRepository.AddOrUpdateAsync(_mapper.Map<Item>(entityModel));

            _loggerService.LogInformation("end - Create Item");

            return Ok(_mapper.Map<ItemModel>(newEntity));
        }
    }
}
