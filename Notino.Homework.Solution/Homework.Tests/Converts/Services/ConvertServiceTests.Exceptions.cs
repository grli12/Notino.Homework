using Homework.Adapters;
using Homework.Adapters.Resolvers.Exceptions;
using Homework.Adapters.Shared.Exceptions;
using Homework.Brokers.Storages.Exceptions;
using Homework.Models;
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
        public async Task ShouldThrowUnsupportedConvertExceptionOnConvertWhenConvertAdapterNotFoundExceptionIsThrownAndLogItAsync()
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
        public async Task ShouldThrowUnsupportedConvertExceptionOnConvertToWhenConvertAdapterNotFoundExceptionIsThrownAndLogItAsync()
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
                resolver.Resolve(It.IsAny<string>()),
                    Times.Exactly(2));

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.convertAdapterResolverMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowConvertFailedExceptionOnConvertWhenAdapterConvertToDocumentFailedExceptionIsThrownAndLogItAsync()
        {
            //given
            var someInnerException =
                new Exception("someInnerException");

            byte[] dummyData = new byte[10];

            var adapterConvertToDocumentFailedException =
                new AdapterConvertToDocumentFailedException(someInnerException);

            Mock<IConvertAdapter> convertAdapterToMock =
                new Mock<IConvertAdapter>();

            convertAdapterToMock.Setup(adapter => 
                adapter.ConvertToDocument(It.IsAny<string>()))
                    .Throws(adapterConvertToDocumentFailedException);

            this.convertAdapterResolverMock.Setup(resolver =>
                resolver.Resolve(It.IsAny<string>()))
                    .Returns(convertAdapterToMock.Object);

            var expectedConvertFailedException =
                new ConvertFailedException(adapterConvertToDocumentFailedException);

            //when

            await Assert.ThrowsAsync<ConvertFailedException>(() =>
                this.convertService.ConvertAsync(
                    keyFrom: It.IsAny<string>(),
                    keyTo: It.IsAny<string>(),
                    fileData: dummyData,
                    targetPath: It.IsAny<string>()));

            //then
            this.loggingBrokerMock.Verify(logginBroker =>
                logginBroker.LogError(It.Is(SameExceptionAs(
                    expectedConvertFailedException))),
                        Times.Once());

            this.convertAdapterResolverMock.Verify(resolver =>
                resolver.Resolve(It.IsAny<string>()),
                    Times.Between(1,2, Moq.Range.Inclusive));

            this.storageBrokerMock.Verify(storage =>
                storage.WriteTextToFileAsync(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Never());

            convertAdapterToMock.Verify(adapter =>
                adapter.ConvertToDocument(It.IsAny<string>()),
                    Times.Once());
                    

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.convertAdapterResolverMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowConvertFailedExceptionOnConvertWhenAdapterConvertFromDocumentFailedExceptionIsThrownAndLogItAsync()
        {
            //given
            var someInnerException =
                new Exception("someInnerException");

            byte[] dummyData = new byte[10];

            var adapterConvertFromDocumentFailedException =
                new AdapterConvertFromDocumentFailedException(someInnerException);

            Mock<IConvertAdapter> convertAdapterFromDocumentMock =
                new Mock<IConvertAdapter>();

            convertAdapterFromDocumentMock.Setup(adapter =>
                adapter.ConvertToText(It.IsAny<Document>()))
                    .Throws(adapterConvertFromDocumentFailedException);

            this.convertAdapterResolverMock.Setup(resolver =>
                resolver.Resolve(It.IsAny<string>()))
                    .Returns(convertAdapterFromDocumentMock.Object);

            var expectedConvertFailedException =
                new ConvertFailedException(adapterConvertFromDocumentFailedException);

            //when
            await Assert.ThrowsAsync<ConvertFailedException>(() =>
                this.convertService.ConvertAsync(
                    keyFrom: It.IsAny<string>(),
                    keyTo: It.IsAny<string>(),
                    fileData: dummyData,
                    targetPath: It.IsAny<string>()));

            //then
            this.loggingBrokerMock.Verify(logginBroker =>
                logginBroker.LogError(It.Is(SameExceptionAs(
                    expectedConvertFailedException))),
                        Times.Once());

            this.convertAdapterResolverMock.Verify(resolver =>
                resolver.Resolve(It.IsAny<string>()),
                    Times.Between(1, 2, Moq.Range.Inclusive));

            this.storageBrokerMock.Verify(storage =>
                storage.WriteTextToFileAsync(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Never());

            convertAdapterFromDocumentMock.Verify(adapter =>
                adapter.ConvertToText(It.IsAny<Document>()),
                    Times.Once());

            convertAdapterFromDocumentMock.Verify(adapter =>
                adapter.ConvertToDocument(It.IsAny<string>()),
                    Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.convertAdapterResolverMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowConvertedFileSaveFailedExceptionOnConvertWhenFileAlreadyExistsExceptionIsThrownAndLogItAsync()
        {
            //given
            string somePath = "somePath";
            byte[] dummyData = new byte[10];

            var fileAlreadyExistsException =
                new FileAlreadyExistsException(somePath);

            var expectedConvertedFileSaveFailedException =
                new ConvertedFileSaveFailedException(fileAlreadyExistsException);

            Mock<IConvertAdapter> someConvertAdapter =
                new Mock<IConvertAdapter>();

            this.convertAdapterResolverMock.Setup(resolver =>
                resolver.Resolve(It.IsAny<string>()))
                    .Returns(someConvertAdapter.Object);

            this.storageBrokerMock.Setup(storage =>
                storage.WriteTextToFileAsync(It.IsAny<string>(), somePath))
                    .Throws(fileAlreadyExistsException);

            //when
            await Assert.ThrowsAsync<ConvertedFileSaveFailedException>(() =>
                this.convertService.ConvertAsync(It.IsAny<string>(), It.IsAny<string>(), dummyData, somePath));

            //then
             this.loggingBrokerMock.Verify(logginBroker =>
                logginBroker.LogError(It.Is(SameExceptionAs(
                    expectedConvertedFileSaveFailedException))),
                        Times.Once());

            this.storageBrokerMock.Verify(storageBroker =>
                storageBroker.WriteTextToFileAsync(It.IsAny<string>(), somePath),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
