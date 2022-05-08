namespace Homework.Adapters.Shared.Exceptions
{
    public class AdapterConvertToDocumentFailedException : Exception
    {
        public AdapterConvertToDocumentFailedException(Exception innerException) 
            : base(message: "Convert to document failed.", innerException)
        {

        }
    }
}
