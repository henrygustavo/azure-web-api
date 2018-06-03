namespace Azure.Service.Implementations
{
    using Interfaces;
    using Newtonsoft.Json.Linq;
    public abstract class BaseService
    {
        private readonly ISecretKeyProvider _secretKeyProvider;

        private readonly string _vaultName;
        private string _vaultValue;

        protected BaseService(ISecretKeyProvider secretKeyProvider, string vaultName)
        {
            _vaultName = vaultName;
            _secretKeyProvider = secretKeyProvider;
        }

        protected string GetSecretValue(string attribute)
        {
            if (string.IsNullOrEmpty(_vaultValue))
            {

                _vaultValue = _secretKeyProvider.GetSecret(_vaultName);
            }

            var jsonObj = JObject.Parse(_vaultValue);
            return (string)jsonObj[attribute];

        }
    }
}
