namespace Homework.Services.Converts.Exceptions
{
    public class FileDataValidationException : Exception
    {
        public FileDataValidationException()
            : base(message: "File is null or empty.")
        {

        }
    }
}
