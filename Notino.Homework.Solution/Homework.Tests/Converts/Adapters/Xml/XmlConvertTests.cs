using FluentAssertions;
using Homework.Adapters.Xmls;
using Homework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tynamix.ObjectFiller;
using Xunit;

namespace Homework.Tests.Converts.Adapters.Xml
{
    public class XmlConvertTests
    {
        private readonly IXmlConvertAdapter xmlConvertAdapter;

        public XmlConvertTests()
        {
            xmlConvertAdapter = new XmlConvertAdapter();
        }

        [Fact]
        public void ShouldConvertDocumentToXmlText()
        {
            //given
            Document validDocument = GenerateRandomDocument();
            XDocument xDoc = BuildXDocument(validDocument);
            string expectedXmlText = xDoc.ToString();

            //when
            string convertedText = this.xmlConvertAdapter.ConvertToText(validDocument);

            //then
            convertedText.Should().BeEquivalentTo(expectedXmlText);
        }

        [Fact]
        public void ShouldConvertXmlTextToDocument()
        {
            //given
            string text = GetRandomText();
            string title = GetRandomText();

            Document expectedDocument = new Document
            {
                Title = title,
                Text = text
            };

            string inputXmlText = BuildXDocument(expectedDocument).ToString(); 

            //when
            Document convertedDocument = this.xmlConvertAdapter.ConvertToDocument(inputXmlText);

            //then
            convertedDocument.Should().BeEquivalentTo(expectedDocument);
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

        private static XDocument BuildXDocument(Document document)
        {
            return new XDocument(
                new XElement("Root",
                    new XElement(nameof(document.Title), document.Title),
                           new XElement(nameof(document.Text), document.Text)));
        }
    }
}
