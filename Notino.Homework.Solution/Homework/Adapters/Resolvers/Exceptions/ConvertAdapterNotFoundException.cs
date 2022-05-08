namespace Homework.Adapters.Resolvers.Exceptions
{
    public class ConvertAdapterNotFoundException : Exception
    {
        public ConvertAdapterNotFoundException(string key)
            : base(message: $"Convert adapter was not found for key {key}.")
        {

        }
    }
}
