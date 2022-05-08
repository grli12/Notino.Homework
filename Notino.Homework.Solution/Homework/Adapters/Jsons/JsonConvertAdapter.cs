using Homework.Models;
using Newtonsoft.Json;

namespace Homework.Adapters.Jsons
{
    public class JsonConvertAdapter : IJsonConvertAdapter
    {
        public Document ConvertToDocument(string text)
        {
            Document? document = JsonConvert.DeserializeObject<Document>(text);

            return document!;
        }

        public string ConvertToText(Document document)
        {
            return JsonConvert.SerializeObject(document);
        }
    }
}
