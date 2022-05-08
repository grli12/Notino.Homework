using Homework.Models;
using Newtonsoft.Json;

namespace Homework.Adapters.Jsons
{
    public class JsonConvertAdapter : IJsonConvertAdapter
    {
        public Document ConvertToDocument(string text)
        {
            throw new NotImplementedException();
        }

        public string ConvertToText(Document document)
        {
            return JsonConvert.SerializeObject(document);
        }
    }
}
