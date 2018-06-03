namespace Azure.Service.Interfaces
{
    using System.Threading.Tasks;

    public interface ISecretKeyProvider
    {
        string GetSecret(string secretName);

        string GetSecretJson(string secretName, string attribute);

        Task<string> GetSecretAsync(string secretName);

        Task<string> GetSecretJsonAsync(string secretName, string attribute);
    }
}
