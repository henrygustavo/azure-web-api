namespace Azure.WebApi.Controllers
{
    using Newtonsoft.Json;
    using Service.Interfaces;
    using Swashbuckle.Swagger.Annotations;
    using System.Net;
    using System.Web.Http;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Models;
    using System.Collections.Generic;
    using Repository.Interfaces;
    using Entity;

    [Authorize]
    public class FacesController : BaseApiController
    {
        private readonly ICognitiveService _cognitiveService;
        private readonly IImageFaceRepository _imageFaceRepository;
        private readonly IStorageService _storageService;
        public FacesController(ICognitiveService cognitiveService,
                               IStorageService storageService, IMapper mapper,
                               IImageFaceRepository imageFaceRepository,
                               ILoggerService loggerService): base(mapper, loggerService)
        {
            _cognitiveService = cognitiveService;
            _storageService = storageService;
            _imageFaceRepository = imageFaceRepository;
        }

        // GET api/faces
        [SwaggerOperation("GetAll")]
        public async Task<IHttpActionResult> Get()
        {
            _loggerService.LogInformation("start - Get faces");

            var entities = await _imageFaceRepository.GetAllAsync();

            _loggerService.LogInformation("end - Get faces");

            List<FaceModel> imageList = entities.Select(entity => 
                                         new FaceModel(entity.Id, entity.ImagePath.ToString(),
                                         _mapper.Map<List<FaceAttributeModel>>(entity.FaceAttributes))).ToList();


            _loggerService.LogInformation("end - Get faces");

            return Ok(imageList);
        }

        [SwaggerOperation("Get")]
        public async Task<IHttpActionResult> Get(string id)
        {
            _loggerService.LogInformation("start - Get face");

            var entity = await _imageFaceRepository.GetByIdAsync(id);

            _loggerService.LogInformation("end - Get face");

            return Ok(new FaceModel(entity.Id, entity.ImagePath.ToString(),
                _mapper.Map<List<FaceAttributeModel>>(entity.FaceAttributes)));
        }

        [HttpPost]
        [SwaggerOperation("Upload")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<IHttpActionResult> Upload()
        {
            _loggerService.LogInformation("start - upload faces");

            if (!System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                _loggerService.LogError("Error- upload faces");
                return BadRequest("error upload");
            }

            var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadedImage"];

            if (httpPostedFile == null)
            {
                _loggerService.LogError("Error- upload faces");
                return BadRequest("error upload");
            }

            var fileExtension = System.IO.Path.GetExtension(httpPostedFile.FileName);

            _loggerService.LogInformation(
                $"upload faces, fileName : {httpPostedFile.FileName}, extension : {fileExtension}");

            var imagePath = await _storageService.AddImageAsync(httpPostedFile.InputStream, fileExtension);

            ImageFace imageFace = new ImageFace
            {
                ImagePath = imagePath,
                FaceAttributes = await _cognitiveService.DetectImageFaceAttributes(imagePath.ToString())
            };

            _loggerService.LogInformation($"end - faces attributes : { JsonConvert.SerializeObject(imageFace.FaceAttributes)}");

            imageFace = await _imageFaceRepository.AddOrUpdateAsync(imageFace);

            _loggerService.LogInformation("end - upload faces");

            return Ok(new FaceModel(imageFace.Id, imageFace.ImagePath.ToString(),
                _mapper.Map<List<FaceAttributeModel>>(imageFace.FaceAttributes)));

        }
    }
}
