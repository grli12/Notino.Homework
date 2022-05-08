namespace Homework.Brokers.Loggings
{
    public interface ILoggingBroker
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(Exception exception);

    }
}
