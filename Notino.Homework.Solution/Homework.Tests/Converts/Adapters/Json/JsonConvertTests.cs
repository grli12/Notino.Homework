using FluentAssertions;
using Homework.Adapters.Jsons;
using Homework.Adapters.Shared.Exceptions;
using Homework.Models;
using Newtonsoft.Json;
using Tynamix.ObjectFiller;
using Xunit;

namespace Homework.Tests.Converts.Adapters.Json
{
    public partial class JsonConvertTests
    {
        private readonly IJsonConvertAdapter jsonConvertAdapter;

        public JsonConvertTests()
        {
            this.jsonConvertAdapter = new JsonConvertAdapter();
        }

        [Fact]
        public void ShouldConvertDocumentToJsonText()
        {
            //given
            Document givenDocument = GenerateRandomDocument();
            string expectedJsonText = JsonConvert.SerializeObject(givenDocument);

            //when
            string convertedText = this.jsonConvertAdapter.ConvertToText(givenDocument);

            //then
            convertedText.Should().BeEquivalentTo(expectedJsonText);
        }

        [Fact]
        public void ShouldConvertJsonTextToDocument()
        {
            //given
            Document originalDocument = GenerateRandomDocument();
            string text = JsonConvert.SerializeObject(originalDocument);
            Document expectedDocument = originalDocument;
            
            //when
            Document convertedDocument 
                = this.jsonConvertAdapter.ConvertToDocument(text);

            //then
            convertedDocument.Should().BeEquivalentTo(expectedDocument);
        }

        [Fact]
        public void ShouldThrowJsonToDocumentConvertFailedExcpetionWhenInvalidJsonTextIsGiven()
        {
            //given
            Document originalDocument = GenerateRandomDocument();
            string validJsonText = JsonConvert.SerializeObject(originalDocument);

            //when
            string invalidJsonText = validJsonText.Replace("{", ".");

            //then
            Assert.Throws<AdapterConvertToDocumentFailedException>(() =>
                this.jsonConvertAdapter.ConvertToDocument(invalidJsonText));
        }

        private static string GetRandomText() => new MnemonicString().GetValue();

        private static Document GenerateRandomDocument()
        {
            var document = new Document
            {
                Text = GetRandomText(),
                Title = GetRandomText()
            };

            return document;
        }
    }
}
