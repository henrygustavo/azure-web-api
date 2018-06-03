namespace Azure.Repository.Implementations
{
    using Entity;
    using Interfaces;

    public class ImageFaceRepository: Repository<ImageFace>, IImageFaceRepository
    {
        public ImageFaceRepository(IDataBaseManager dataBaseManager) : base(dataBaseManager)
        {
        }
    }
}
