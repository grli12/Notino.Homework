using Homework.Adapters;
using Homework.Adapters.Resolvers.Exceptions;
using Homework.Services.Converts.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Homework.Tests.Converts.Services
{
    public partial class ConvertServiceTests
    {
        [Fact]
        public async void ShouldThrowUnsupportedConvertExceptionOnConvertWhenConvertAdapterNotFoundExceptionIsThrown()
        {
            //given;
            string keyFrom = "keyFrom";

            var convertAdapterNotFoundException =
                new ConvertAdapterNotFoundException(keyFrom);

            var expectedUnsupportedConvertException =
                new UnsupportedConvertException(convertAdapterNotFoundException);

            this.convertAdapterResolverMock.Setup(resolver =>
                resolver.Resolve(keyFrom))
                    .Throws(convertAdapterNotFoundException);

            //when
            await Assert.ThrowsAsync<UnsupportedConvertException>(() =>
                this.convertService.ConvertAsync(keyFrom, It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()));

            //then

            this.loggingBrokerMock.Verify(logginBroker =>
                logginBroker.LogError(It.Is(SameExceptionAs(
                    expectedUnsupportedConvertException))),
                        Times.Once());

            this.convertAdapterResolverMock.Verify(resolver =>
                resolver.Resolve(keyFrom),
                    Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.convertAdapterResolverMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowUnsupportedConvertExceptionOnConvertToWhenConvertAdapterNotFoundExceptionIsThrown()
        {
            //given;
            string keyTo = "keyTo";

            var convertAdapterNotFoundException =
                new ConvertAdapterNotFoundException(keyTo);

            var expectedUnsupportedConvertException =
                new UnsupportedConvertException(convertAdapterNotFoundException);

            this.convertAdapterResolverMock.Setup(resolver =>
                resolver.Resolve(keyTo))
                    .Throws(convertAdapterNotFoundException);

            //when
            await Assert.ThrowsAsync<UnsupportedConvertException>(() =>
                this.convertService.ConvertAsync(It.IsAny<string>(),keyTo, It.IsAny<byte[]>(), It.IsAny<string>()));

            //then

            this.loggingBrokerMock.Verify(logginBroker =>
                logginBroker.LogError(It.Is(SameExceptionAs(
                    expectedUnsupportedConvertException))),
                        Times.Once());

            this.convertAdapterResolverMock.Verify(resolver =>
                resolver.Resolve(keyTo),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
