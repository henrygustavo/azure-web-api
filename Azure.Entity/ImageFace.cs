namespace Azure.Entity
{
    using System.Collections.Generic;
    using Microsoft.ProjectOxford.Face.Contract;

    public class ImageFace : Image
    {
        public ICollection<Face> FaceAttributes { get; set; }
    }
}
