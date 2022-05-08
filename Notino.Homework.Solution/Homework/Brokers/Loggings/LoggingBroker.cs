namespace Homework.Brokers.Loggings
{
    public class LoggingBroker : ILoggingBroker
    {
        private readonly ILogger<LoggingBroker> logger;

        public LoggingBroker(ILogger<LoggingBroker> logger) =>
            this.logger = logger;

        public void LogError(Exception exception)
        {
            logger.LogError(exception, exception.Message);
        }

        public void LogInfo(string message)
        {
            logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            logger.LogWarning(message);
        }
    }
}
