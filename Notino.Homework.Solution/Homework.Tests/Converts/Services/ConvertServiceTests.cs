using FluentAssertions;
using Homework.Adapters;
using Homework.Adapters.Resolvers;
using Homework.Brokers.Loggings;
using Homework.Brokers.Storages;
using Homework.Models;
using Homework.Services.Converts;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;
using Xunit;

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

        [Fact]
        public async Task ShouldConvertAsync()
        {
            //given
            byte[] dummyData = new byte[10];
            Document randomDocument = GenerateRandomDocument();
            string textFromDocument = GetRandomText();
            string targetPath = "somePath";
            string expectedPathToConvertedFile = targetPath;
            string someKey = "someKey";

            Mock<IConvertAdapter> convertAdapterMock =
                new Mock<IConvertAdapter>();

            convertAdapterMock.Setup(adapter =>
                adapter.ConvertToDocument(It.IsAny<string>()))
                    .Returns(randomDocument);

            convertAdapterMock.Setup(adapter =>
                adapter.ConvertToText(randomDocument))
                    .Returns(textFromDocument);

            this.convertAdapterResolverMock.Setup(resolver =>
                resolver.Resolve(It.IsAny<string>()))
                    .Returns(convertAdapterMock.Object);

            this.storageBrokerMock.Setup(storage =>
                storage.WriteTextToFileAsync(textFromDocument, targetPath))
                    .ReturnsAsync(expectedPathToConvertedFile);
            //when
            string createdFilePath =
                await this.convertService.ConvertAsync(someKey, someKey, dummyData, targetPath);

            //then
            createdFilePath.Should().BeEquivalentTo(expectedPathToConvertedFile);

            convertAdapterMock.Verify(adapter =>
                adapter.ConvertToText(randomDocument),
                    Times.Once);

            convertAdapterMock.Verify(adapter =>
                adapter.ConvertToDocument(It.IsAny<string>()),
                    Times.Once);

            this.storageBrokerMock.Verify(storage =>
                storage.WriteTextToFileAsync(textFromDocument, targetPath),
                    Times.Once);

            this.convertAdapterResolverMock.Verify(resolver =>
                resolver.Resolve(It.IsAny<string>()),
                 Times.Exactly(2));

            this.loggingBrokerMock.Verify(logging =>
                logging.LogError(It.IsAny<Exception>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.convertAdapterResolverMock.VerifyNoOtherCalls();
        }

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException!.InnerException!.Message == expectedException!.InnerException!.Message;
        }

        private static Document GenerateRandomDocument()
        {
            var document = new Document
            {
                Text = GetRandomText(),
                Title = GetRandomText()
            };

            return document;
        }

        private static string GetRandomText() => new MnemonicString().GetValue();
    }
}
