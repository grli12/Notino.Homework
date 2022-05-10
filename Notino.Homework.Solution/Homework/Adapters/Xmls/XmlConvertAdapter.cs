using Homework.Adapters.Shared.Exceptions;
using Homework.Models;
using System.Xml.Linq;

namespace Homework.Adapters.Xmls
{
    public class XmlConvertAdapter : IXmlConvertAdapter
    {
        public Document ConvertToDocument(string input)
        {
            try
            {
                XDocument xDocument = XDocument.Parse(input);

                var document = new Document
                {
                    Title = GetValueFromElement(xDocument, nameof(Document.Title)),
                    Text = GetValueFromElement(xDocument, nameof(Document.Text)),
                };

                return document;
            }
            catch(Exception ex)
            {
                throw new AdapterConvertToDocumentFailedException(innerException: ex);
            }
            
        }

        public string ConvertToText(Document document)
        {
            XDocument xDoc = new XDocument(
                new XElement("Root",
                    new XElement(nameof(document.Title), document.Title),
                           new XElement(nameof(document.Text), document.Text)));

            return xDoc.ToString();
        }

        private string GetValueFromElement(XDocument xDocuement, string elementName)
        {
            if(xDocuement.Root != null)
            {
                XElement? element = xDocuement.Root.Element(elementName);
                
                if(element != null)
                {
                    return element.Value;
                }
            }

            return string.Empty;
        }
    }
}
