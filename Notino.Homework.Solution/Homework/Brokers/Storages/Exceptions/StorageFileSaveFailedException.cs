namespace Homework.Brokers.Storages.Exceptions
{
    public class StorageFileSaveFailedException : Exception
    {
        public StorageFileSaveFailedException(Exception innerException)
            : base(message: "File could not be saved at target path.", innerException)
        {

        }
    }
}
