namespace Homework.Services.Converts.Exceptions
{
    public class ConvertFailedException : Exception
    {
        public ConvertFailedException(Exception innerException)
            : base(message: "Convert failed, see inner exception for details.", innerException)
        {

        }
    }
}
