namespace Azure.Entity
{
    using Microsoft.ProjectOxford.Vision.Contract;

    public class ImageRecognition : Image
    {
       public AnalysisResult AnalysisResult { get; set; }
    }
}
