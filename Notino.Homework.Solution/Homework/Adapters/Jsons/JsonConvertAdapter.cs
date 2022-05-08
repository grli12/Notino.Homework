using Homework.Adapters.Shared.Exceptions;
using Homework.Models;
using Newtonsoft.Json;

namespace Homework.Adapters.Jsons
{
    public class JsonConvertAdapter : IJsonConvertAdapter
    {
        public Document ConvertToDocument(string text)
        {
            try
            {
                Document? document = JsonConvert.DeserializeObject<Document>(text);

                return document!;
            }
            catch (Exception ex)
            {
                throw new AdapterConvertToDocumentFailedException(innerException: ex);
            }
            
        }

        public string ConvertToText(Document document)
        {
            return JsonConvert.SerializeObject(document);
        }
    }
}
