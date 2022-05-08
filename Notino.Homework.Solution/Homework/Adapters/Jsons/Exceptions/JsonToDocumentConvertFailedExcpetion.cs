namespace Homework.Adapters.Jsons.Exceptions
{
    public class JsonToDocumentConvertFailedExcpetion : Exception
    {
        public JsonToDocumentConvertFailedExcpetion(Exception innerException)
            : base(message: "Convert from json to document failed.", innerException)
        {

        }
    }
}
