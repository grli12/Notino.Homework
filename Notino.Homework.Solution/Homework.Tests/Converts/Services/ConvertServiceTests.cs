using Homework.Adapters.Resolvers;
using Homework.Brokers.Loggings;
using Homework.Brokers.Storages;
using Homework.Services.Converts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Homework.Tests.Converts.Services
{
    public partial class ConvertServiceTests
    {
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IConvertAdapterResolver> convertAdapterResolverMock;
        private readonly IConvertService convertService;

        public ConvertServiceTests()
        {
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.convertAdapterResolverMock = new Mock<IConvertAdapterResolver>();

            this.convertService = new ConvertService(
                convertAdapterResolver: this.convertAdapterResolverMock.Object,
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object
                );
        }

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException!.InnerException!.Message == expectedException!.InnerException!.Message;
        }
    }
}
