namespace Homework.Services.Converts.Exceptions
{
    public class ConvertedFileSaveFailedException : Exception
    {
        public ConvertedFileSaveFailedException(Exception innerException)
            : base(message: "The converted file cannot be created.", innerException)
        {

        }
    }
}
