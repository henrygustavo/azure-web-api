namespace Azure.Service.Implementations
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.KeyVault;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Interfaces;
    using Newtonsoft.Json.Linq;

    public class SecretKeyProvider : ISecretKeyProvider
    {
        private readonly string _clientId;
        private readonly string _secret;
        private readonly string _baseUrl;

        public SecretKeyProvider(string clientId, string secret, string baseUrl)
        {
            _clientId = clientId;
            _secret = secret;
            _baseUrl = baseUrl;
        }
        public string GetSecret(string secretName)
        {
            return Task.Run(() => GetSecretAsync(secretName)).Result;
        }

        public string GetSecretJson(string secretName, string attribute)
        {
            return Task.Run(() => GetSecretJsonAsync(secretName, attribute)).Result;
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            KeyVaultClient operations = new KeyVaultClient(GetToken);
            var secret = (await operations.GetSecretAsync(_baseUrl, secretName)).Value;
            return secret;
        }

        public async Task<string> GetSecretJsonAsync(string secretName, string attribute)
        {
            KeyVaultClient operations = new KeyVaultClient(GetToken);
            var secretJson = (await operations.GetSecretAsync(_baseUrl, secretName)).Value;

            var jsonObj = JObject.Parse(secretJson);
            return  (string)jsonObj[attribute];
        }

        private async Task<string> GetToken(string authority, string resource, string scope)
        {
            AuthenticationContext authenticationContext = new AuthenticationContext(authority);
            ClientCredential clientCredential1 = new ClientCredential(_clientId, _secret);
            ClientCredential clientCredential2 = clientCredential1;
            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenAsync(resource, clientCredential2);
            if (authenticationResult == null)
                throw new InvalidOperationException("No token.");
            return authenticationResult.AccessToken;
        }
    }
}
