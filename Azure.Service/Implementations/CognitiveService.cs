namespace Azure.Service.Implementations
{
    using Interfaces;
    using System.Threading.Tasks;
    using Microsoft.ProjectOxford.Face;
    using Microsoft.ProjectOxford.Vision;


    public class CognitiveService : BaseService, ICognitiveService
    {
        private readonly IFaceServiceClient _faceClient;
        private readonly VisionServiceClient _visionServiceClient;

        public CognitiveService(ISecretKeyProvider secretKeyProvider, string vaultName):
                                base(secretKeyProvider, vaultName)
        {
            _faceClient = new FaceServiceClient(GetSecretValue("FaceKey"), GetSecretValue("FaceUrl"));

            _visionServiceClient = new VisionServiceClient(GetSecretValue("VisionKey"), GetSecretValue("VisionUrl"));
        }

        public async Task<Microsoft.ProjectOxford.Face.Contract.Face[]> DetectImageFaceAttributes(string imageUrl)
        {
            var attributes = new[]
            {
                FaceAttributeType.Gender,
                FaceAttributeType.Age,
                FaceAttributeType.Smile,
                FaceAttributeType.Emotion,
                FaceAttributeType.Glasses,
                FaceAttributeType.Hair
            };

            return  await _faceClient.DetectAsync(imageUrl, returnFaceId: true,
                         returnFaceLandmarks: true, returnFaceAttributes: attributes);
        }

        public async Task<Microsoft.ProjectOxford.Vision.Contract.AnalysisResult> AnalyzeImage(string imageUrl)
        {
           
            VisualFeature[] visualFeatures = { VisualFeature.Adult, VisualFeature.Categories,
                                                                   VisualFeature.Color, VisualFeature.Description,
                                                                    VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags };
            var analysisResult = await _visionServiceClient.AnalyzeImageAsync(imageUrl, visualFeatures);
            return analysisResult;
        }


    }
}
