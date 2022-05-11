namespace Homework.Services.Converts.Exceptions
{
    public class ConvertValidationException : Exception
    {
        public ConvertValidationException(Exception innerException)
            : base(message: "Convert validation exception occurred.", innerException)
        {
            
        }
    }
}
