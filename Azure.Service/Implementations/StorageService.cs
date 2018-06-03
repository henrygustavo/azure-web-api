namespace Azure.Service.Implementations
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    public class StorageService : BaseService, IStorageService
    {
        private readonly CloudBlobContainer _container;
        private const string _imagePrefix = "img_";

        public StorageService(ISecretKeyProvider secretKeyProvider, string vaultName) :
                              base(secretKeyProvider, vaultName)
        {
            string connectionString = GetSecretValue("StorageConectionString");

            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudBlobClient();
            _container = client.GetContainerReference(GetSecretValue("StorageContainer"));
            _container.CreateIfNotExists();

            var permissions = _container.GetPermissions();
            if (permissions.PublicAccess == BlobContainerPublicAccessType.Off || permissions.PublicAccess == BlobContainerPublicAccessType.Unknown)
            {
                _container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
        }
        public async Task<System.Uri> AddImageAsync(Stream stream, string fileExtension)
        {
            var imageStorageGuid = Guid.NewGuid();
            var fileName = string.Concat(_imagePrefix, imageStorageGuid, fileExtension);
            var imageBlob = _container.GetBlockBlobReference(fileName);
            await imageBlob.UploadFromStreamAsync(stream);

            return imageBlob.Uri;
        }
    }
}
