namespace Azure.WebApi.Models
{
    using System.Collections.Generic;

    public class FaceModel : BaseImageModel
    {
        public List<FaceAttributeModel> FacesAttributes { get; set; }

        public FaceModel(string imageId,string imagePath, List<FaceAttributeModel> facesAttributes)
        {
            ImageId = imageId;
            ImagePath = imagePath;
            FacesAttributes = facesAttributes;
        }

    }
}