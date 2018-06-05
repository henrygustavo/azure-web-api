namespace Azure.Service.Implementations
{
    using Serilog;
    using Interfaces;
    using SerilogWeb.Classic.Enrichers;

    public class LoggerService : BaseService, ILoggerService
    {

        public LoggerService(ISecretKeyProvider secretKeyProvider, string vaultName) : base(secretKeyProvider, vaultName)
        {
  
            LoggerConfiguration loggerConfiguration = new LoggerConfiguration();

#if DEBUG
            loggerConfiguration.WriteTo.File("C:\\temps\\azure.txt", rollingInterval: RollingInterval.Day);
#else
            var appInsightKey = secretKeyProvider.GetSecret(vaultName);
            
            loggerConfiguration.WriteTo.ApplicationInsightsTraces(appInsightKey);
#endif
            Log.Logger = loggerConfiguration.Enrich.With<HttpRequestIdEnricher>().CreateLogger();
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
