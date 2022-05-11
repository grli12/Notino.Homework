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
            string randomKey = GetRandomText();
            string randomPath = GetRandomText();
            byte[] dummyData = new byte[10];


            var convertAdapterNotFoundException =
                new ConvertAdapterNotFoundException(keyFrom);

            var expectedUnsupportedConvertException =
                new UnsupportedConvertException(convertAdapterNotFoundException);

            this.convertAdapterResolverMock.Setup(resolver =>
                resolver.Resolve(keyFrom))
                    .Throws(convertAdapterNotFoundException);

            //when
            await Assert.ThrowsAsync<UnsupportedConvertException>(() =>
                this.convertService.ConvertAsync(keyFrom, randomKey, dummyData, randomPath));

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
            string randomKey = GetRandomText();
            string randomPath = GetRandomText();
            byte[] dummyData = new byte[10];

            var convertAdapterNotFoundException =
                new ConvertAdapterNotFoundException(keyTo);

            var expectedUnsupportedConvertException =
                new UnsupportedConvertException(convertAdapterNotFoundException);

            this.convertAdapterResolverMock.Setup(resolver =>
                resolver.Resolve(keyTo))
                    .Throws(convertAdapterNotFoundException);

            //when
            await Assert.ThrowsAsync<UnsupportedConvertException>(() =>
                this.convertService.ConvertAsync(randomKey,keyTo, dummyData, randomPath));

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
            string randomKey = GetRandomText();
            string randomPath = GetRandomText();

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
                    keyFrom: randomKey,
                    keyTo: randomKey,
                    fileData: dummyData,
                    targetPath: randomPath));

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
            string randomKey = GetRandomText();
            string randomPath = GetRandomText();

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
                    keyFrom: randomKey,
                    keyTo: randomKey,
                    fileData: dummyData,
                    targetPath: randomPath));

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
            string randomKey = GetRandomText();
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
                this.convertService.ConvertAsync(randomKey, randomKey, dummyData, somePath));

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

        [Fact]
        public async Task ShouldThrowConvertedFileSaveFailedExceptionOnConvertWhenStorageFileSaveFailedExceptionIsThrownAndLogItAsync()
        {
            //given
            string randomKey = GetRandomText();
            string somePath = "somePath";
            byte[] dummyData = new byte[10];
            var someInnerException = new Exception("Some inner exception");

            var storageFileSaveFailedException =
                new StorageFileSaveFailedException(someInnerException);

            var expectedConvertedFileSaveFailedException =
                new ConvertedFileSaveFailedException(storageFileSaveFailedException);

            Mock<IConvertAdapter> someConvertAdapter =
                new Mock<IConvertAdapter>();

            this.convertAdapterResolverMock.Setup(resolver =>
                resolver.Resolve(It.IsAny<string>()))
                    .Returns(someConvertAdapter.Object);

            this.storageBrokerMock.Setup(storage =>
                storage.WriteTextToFileAsync(It.IsAny<string>(), somePath))
                    .Throws(storageFileSaveFailedException);

            //when
            await Assert.ThrowsAsync<ConvertedFileSaveFailedException>(() =>
                this.convertService.ConvertAsync(randomKey, randomKey, dummyData, somePath));

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
