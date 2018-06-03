namespace Azure.Repository.Implementations
{
    using Entity;
    using Interfaces;

    public class ImageRecognitionRepository : Repository<ImageRecognition>, IImageRecognitionRepository
    {
        public ImageRecognitionRepository(IDataBaseManager dataBaseManager) : base(dataBaseManager)
        {
        }
    }
}
