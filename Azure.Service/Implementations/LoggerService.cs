namespace Azure.Service.Implementations
{
    using Serilog;
    using Interfaces;

    public class LoggerService : BaseService, ILoggerService
    {

        public LoggerService(ISecretKeyProvider secretKeyProvider, string vaultName) : base(secretKeyProvider, vaultName)
        {
            var appInsightKey = secretKeyProvider.GetSecret(vaultName);

            Log.Logger = new LoggerConfiguration()

                .MinimumLevel.Verbose()
                .WriteTo
                .ApplicationInsightsEvents(appInsightKey)
                .CreateLogger();
        }

        public void LogInformation(string message)
        {
            Log.Information(message);
        }

        public void LogError(string message)
        {
            Log.Error(message);
        }

        public void LogDebug(string message)
        {
            Log.Debug(message);
        }

    }
}
