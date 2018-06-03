namespace Azure.WebApi.Controllers
{
    using System.Web.Http;
    using Repository.Interfaces;
    using Service.Interfaces;
    using AutoMapper;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Swashbuckle.Swagger.Annotations;
    using Entity;
    using Models;
    using System.Collections.Generic;

    public class ImagesController : BaseController
    {
        private readonly ICognitiveService _cognitiveService;
        private readonly IImageRecognitionRepository _imageRecognitionRepository;
        private readonly IStorageService _storageService;

        public ImagesController(ICognitiveService cognitiveService,
            IStorageService storageService, IMapper mapper,
            IImageRecognitionRepository imageRecognitionRepository,
            ILoggerService loggerService) : base(mapper, loggerService)
        {
            _cognitiveService = cognitiveService;
            _storageService = storageService;
            _imageRecognitionRepository = imageRecognitionRepository;
        }

        // GET api/faces
        [SwaggerOperation("GetAll")]
        public async Task<IHttpActionResult> Get()
        {
            _loggerService.LogInformation("start - Get images");

            var entities = await _imageRecognitionRepository.GetAllAsync();

            _loggerService.LogInformation("end - Get images");

            return Ok(_mapper.Map<List<ImageRecognitionModel>>(entities));
        }

        [HttpPost]
        [SwaggerOperation("Upload")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<IHttpActionResult> Upload()
        {
            _loggerService.LogInformation("start - upload image");

            if (!System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                _loggerService.LogError("Error- upload image");
                return BadRequest("error upload");
            }

            var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadedImage"];

            if (httpPostedFile == null)
            {
                _loggerService.LogError("Error- upload image");
                return BadRequest("error upload");
            }

            var fileExtension = System.IO.Path.GetExtension(httpPostedFile.FileName);

            _loggerService.LogInformation(
                $"upload image, fileName : {httpPostedFile.FileName}, extension : {fileExtension}");

            var imagePath = await _storageService.AddImageAsync(httpPostedFile.InputStream, fileExtension);

            ImageRecognition imageRecognition = new ImageRecognition
            {
                ImagePath = imagePath,
                AnalysisResult = await _cognitiveService.AnalyzeImage(imagePath.ToString())
            };

            _loggerService.LogInformation($"end - image attributes : { JsonConvert.SerializeObject(imageRecognition.AnalysisResult)}");

            imageRecognition = await _imageRecognitionRepository.AddOrUpdateAsync(imageRecognition);

            _loggerService.LogInformation("end - upload image");

            return Ok( _mapper.Map<ImageRecognitionModel>(imageRecognition));
        }
    }
}
