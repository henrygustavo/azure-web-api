namespace Azure.Service.Interfaces
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IStorageService
    {
        Task<System.Uri> AddImageAsync(Stream stream, string fileExtension);
    }
}
