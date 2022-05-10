using Homework.Adapters;
using Homework.Adapters.Resolvers;
using Homework.Adapters.Resolvers.Exceptions;
using Homework.Adapters.Shared.Exceptions;
using Homework.Brokers.Loggings;
using Homework.Brokers.Storages;
using Homework.Constants;
using Homework.Models;
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

        public async Task<string> ConvertAsync(string keyFrom, string keyTo, byte[] fileData, string targetPath)
        {
            try
            {
                IConvertAdapter convertFromAdapter =
                    this.convertAdapterResolver.Resolve(keyFrom);

                IConvertAdapter convertToAdapter =
                    this.convertAdapterResolver.Resolve(keyTo);

                string convertedText = await ConvertFileAsync(convertFromAdapter, convertToAdapter, fileData);

                return string.Empty;
            }
            catch (ConvertAdapterNotFoundException convertAdapterNotFoundException)
            {
                throw CreateAndLogUnsupportedConvertException(convertAdapterNotFoundException);
            }
            catch(AdapterConvertToDocumentFailedException convertToDocumentFailedException)
            {
                throw CreateAndLogConvertFailedException(convertToDocumentFailedException);
            }
            catch(AdapterConvertFromDocumentFailedException convertFromDocumentFailedException)
            {
                throw CreateAndLogConvertFailedException(convertFromDocumentFailedException);
            }

        }

        private async Task<string> ConvertFileAsync(IConvertAdapter fromAdapter, IConvertAdapter toAdapter, byte[] fileData)
        {
            using(var memStream = new MemoryStream(fileData))
            {
                using(var streamReader = new StreamReader(memStream))
                {
                    string inputText = await streamReader.ReadToEndAsync();
                    Document document = fromAdapter.ConvertToDocument(inputText);
                    string outputText = toAdapter.ConvertToText(document);

                    return outputText;
                }
            }
        }

        private UnsupportedConvertException CreateAndLogUnsupportedConvertException(Exception innerException)
        {
            var unsupportedConvertException = 
                new UnsupportedConvertException(innerException);

            this.loggingBroker.LogError(unsupportedConvertException);

            return unsupportedConvertException;
        }

        private ConvertFailedException CreateAndLogConvertFailedException(Exception innerException)
        {
            var convertFailedException =
                new ConvertFailedException(innerException);

            this.loggingBroker.LogError(convertFailedException);

            return convertFailedException;
        }
    }
}
