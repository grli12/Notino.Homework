namespace Homework.Brokers.Storages.Exceptions
{
    public class StorageFileSaveFailedException : Exception
    {
        public StorageFileSaveFailedException(Exception innerException)
            : base(message: "File cannot be saved. See inner exception for details.", innerException)
        {

        }
    }
}
