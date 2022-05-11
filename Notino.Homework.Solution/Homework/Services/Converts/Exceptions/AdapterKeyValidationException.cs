namespace Homework.Services.Converts.Exceptions
{
    public class AdapterKeyValidationException : Exception
    {
        public AdapterKeyValidationException()
            : base(message: "The adapter key is invalid.")
        {

        }
    }
}
