namespace Azure.Repository.Implementations
{
    using Azure.Service.Interfaces;
    using Interfaces;
    using Newtonsoft.Json.Linq;

    public class DataBaseManager: IDataBaseManager
    {
        private readonly ISecretKeyProvider _secretKeyProvider;
        private readonly string _vaultName;
        private string _vaultValue;
        public DataBaseManager(ISecretKeyProvider secretKeyProvider, string vaultName)
        {
            _secretKeyProvider = secretKeyProvider;
            _vaultName = vaultName;
        }
        public string GetDataBaseEndPoint()
        {
            return GetSecretValue("AccountEndpoint");
        }

        public string GetDataBaseKey()
        {
            return GetSecretValue("AccountKey");
        }

        public string GetDataBaseId()
        {
            return GetSecretValue("DatabaseId");
        }

        private string GetSecretValue(string attribute)
        {
            if(string.IsNullOrEmpty(_vaultValue))
            {

                _vaultValue = _secretKeyProvider.GetSecret(_vaultName);
            }

            var jsonObj = JObject.Parse(_vaultValue);
            return (string)jsonObj[attribute];

        }
    }
}
