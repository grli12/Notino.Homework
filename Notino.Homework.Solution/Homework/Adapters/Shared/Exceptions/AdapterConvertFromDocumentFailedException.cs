namespace Homework.Adapters.Shared.Exceptions
{
    public class AdapterConvertFromDocumentFailedException : Exception
    {
        public AdapterConvertFromDocumentFailedException(Exception  innerException)
            : base(message: "Convert from document failed. See inner exception for details.", innerException)
        {

        }
    }
}
