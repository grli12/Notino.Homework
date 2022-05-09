using Homework.Models;
using System.Xml.Linq;

namespace Homework.Adapters.Xmls
{
    public class XmlConvertAdapter : IXmlConvertAdapter
    {
        public Document ConvertToDocument(string input)
        {
            throw new NotImplementedException();
        }

        public string ConvertToText(Document document)
        {
            XDocument xDoc = new XDocument(
                new XElement("Root",
                    new XElement(nameof(document.Title), document.Title),
                           new XElement(nameof(document.Text), document.Text)));
            return xDoc.ToString();
        }
    }
}
