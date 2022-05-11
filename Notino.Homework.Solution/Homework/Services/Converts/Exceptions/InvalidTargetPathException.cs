namespace Homework.Services.Converts.Exceptions
{
    public class InvalidTargetPathException : Exception
    {
        public InvalidTargetPathException()
            : base(message: "The target path is invalid.")
        {

        }
    }
}
