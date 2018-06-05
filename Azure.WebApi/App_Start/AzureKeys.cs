namespace Azure.WebApi.App_Start
{
    using Helpers;
    using Microsoft.Azure;

    public static class AzureKeys
    {

        public static string KeyVaultClientId => CloudConfigurationManager.GetSetting("VaultClientId");

        public static string KeyVaultBaseUrl => CloudConfigurationManager.GetSetting("VaultBaseUrl");

        public static string KeyVaultSecret => StringCipher.Decrypt(CloudConfigurationManager.GetSetting("VaultSecret"),
                                                    CloudConfigurationManager.GetSetting("VaultSecretDecriptPassword"));


        public static string CognityServicesVaultName => CloudConfigurationManager.GetSetting("AzureCognityServicesVaultName");

        public static string CosmoDBVaultName => CloudConfigurationManager.GetSetting("AzureCosmosDBVaultName");

        public static string StorageVaultName => CloudConfigurationManager.GetSetting("AzureStorageVaultName");

        public static string ApplicatonInsightKey => CloudConfigurationManager.GetSetting("AzureApplicatonInsightKey");

        public static string AuthTokenKey => CloudConfigurationManager.GetSetting("AzureAuthTokenKey");

    }
}