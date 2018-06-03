namespace Azure.Service.Interfaces
{
    public interface ILoggerService
    {
        void LogInformation( string message);

        void LogError(string message);

        void LogDebug(string message);
    }
}
