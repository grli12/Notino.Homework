using Homework.Services.Converts.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Homework.Tests.Converts.Services
{
    public partial class ConvertServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnConvertWhenConvertKeyIsInvalidAndLogItAsync(
            string invalidConvertKey)
        {
            string validConvertKey = GetRandomText();
            byte[] validData = new byte[10];
            string validPath = GetRandomText();
            //given
            var adapterKeyValidationException =
                new AdapterKeyValidationException();

            var expectedConvertValidationException =
                new ConvertValidationException(adapterKeyValidationException);

            //when
            await Assert.ThrowsAsync<ConvertValidationException>(() =>
                this.convertService.ConvertAsync(invalidConvertKey, validConvertKey, validData, validPath));

            //then
            this.loggingBrokerMock.Verify(logging =>
                logging.LogError(It.Is(SameExceptionAs(expectedConvertValidationException))));

            this.convertAdapterResolverMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnConvertWhenConvertKeyToIsInvalidAndLogItAsync(
            string invalidConvertKey)
        {
            string validConvertKey = GetRandomText();
            byte[] validData = new byte[10];
            string validPath = GetRandomText();
            //given
            var adapterKeyValidationException =
                new AdapterKeyValidationException();

            var expectedConvertValidationException =
                new ConvertValidationException(adapterKeyValidationException);

            //when
            await Assert.ThrowsAsync<ConvertValidationException>(() =>
                this.convertService.ConvertAsync(validConvertKey, invalidConvertKey, validData, validPath));

            //then
            this.loggingBrokerMock.Verify(logging =>
                logging.LogError(It.Is(SameExceptionAs(expectedConvertValidationException))));

            this.convertAdapterResolverMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnConvertWhenTargetPathIsInvalidAndLogItAsync(
            string invalidTargetPath)
        {
            string validConvertKey = GetRandomText();
            byte[] validData = new byte[10];
            //given
            var invalidTargetPathException =
                new InvalidTargetPathException();

            var expectedConvertValidationException =
                new ConvertValidationException(invalidTargetPathException);

            //when
            await Assert.ThrowsAsync<ConvertValidationException>(() =>
                this.convertService.ConvertAsync(validConvertKey, validConvertKey, validData, invalidTargetPath));

            //then
            this.loggingBrokerMock.Verify(logging =>
                logging.LogError(It.Is(SameExceptionAs(expectedConvertValidationException))));

            this.convertAdapterResolverMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new byte[0])]
        public async Task ShouldThrowValidationExceptionOnConvertFileDataIsInvalidAndLogItAsync(
            byte[] invalidFileData)
        {
            string validConvertKey = GetRandomText();
            string validPath = GetRandomText();
            //given
            var fileDataValidationException =
                new FileDataValidationException();

            var expectedConvertValidationException =
                new ConvertValidationException(fileDataValidationException);

            //when
            await Assert.ThrowsAsync<ConvertValidationException>(() =>
                this.convertService.ConvertAsync(validConvertKey, validConvertKey, invalidFileData, validPath));

            //then
            this.loggingBrokerMock.Verify(logging =>
                logging.LogError(It.Is(SameExceptionAs(expectedConvertValidationException))));

            this.convertAdapterResolverMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
