using Homework.Models;

namespace Homework.Adapters
{
    public interface IConvertAdapter
    {
        Document ConvertToDocument(string text);
        string ConvertToText(Document document);
    }
}
