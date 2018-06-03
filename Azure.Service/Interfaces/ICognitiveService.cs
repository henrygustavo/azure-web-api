namespace Azure.Service.Interfaces
{
    using System.Threading.Tasks;

    public interface ICognitiveService
    {
        Task<Microsoft.ProjectOxford.Face.Contract.Face[]> DetectImageFaceAttributes(string imageUrl);

        Task<Microsoft.ProjectOxford.Vision.Contract.AnalysisResult> AnalyzeImage(string imageUrl);
    }
}
