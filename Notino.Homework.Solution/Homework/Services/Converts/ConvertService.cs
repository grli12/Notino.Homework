using Homework.Adapters;
using Homework.Adapters.Resolvers;
using Homework.Adapters.Resolvers.Exceptions;
using Homework.Brokers.Loggings;
using Homework.Brokers.Storages;
using Homework.Constants;
using Homework.Services.Converts.Exceptions;

namespace Homework.Services.Converts
{
    public class ConvertService : IConvertService
    {
        private readonly IConvertAdapterResolver convertAdapterResolver;
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public ConvertService(
            IConvertAdapterResolver convertAdapterResolver, 
            IStorageBroker storageBroker, 
            ILoggingBroker loggingBroker)
        {
            this.convertAdapterResolver = convertAdapterResolver;
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public Task<string> ConvertAsync(string keyFrom, string keyTo, byte[] fileData, string targetPath)
        {
            try
            {
                IConvertAdapter convertFromAdapter =
                    this.convertAdapterResolver.Resolve(keyFrom);

                return Task.FromResult(string.Empty);
            }
            catch (ConvertAdapterNotFoundException convertAdapterNotFoundException)
            {
                throw CreateAndLogUnsupportedConvertException(convertAdapterNotFoundException);
            }

        }

        private UnsupportedConvertException CreateAndLogUnsupportedConvertException(Exception innerException)
        {
            var unsupportedConvertException = 
                new UnsupportedConvertException(innerException);

            this.loggingBroker.LogError(unsupportedConvertException);

            return unsupportedConvertException;
        }
    }
}
