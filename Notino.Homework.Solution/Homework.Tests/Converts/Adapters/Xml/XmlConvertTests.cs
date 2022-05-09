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
            XDocument xDoc = new XDocument(
                new XElement("Root", 
                    new XElement(nameof(validDocument.Title), validDocument.Title),
                           new XElement(nameof(validDocument.Text), validDocument.Text)));
            string expectedXmlText = xDoc.ToString();

            //when
            string convertedText = xmlConvertAdapter.ConvertToText(validDocument);

            //then
            convertedText.Should().BeEquivalentTo(expectedXmlText);
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
