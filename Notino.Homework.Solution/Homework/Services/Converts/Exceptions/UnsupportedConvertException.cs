namespace Homework.Services.Converts.Exceptions
{
    public class UnsupportedConvertException : Exception
    {
        public UnsupportedConvertException(Exception innerException)
            : base(message: "Convert type is not supported. See inner exception for details.", innerException)
        {

        }
    }
}
