namespace Homework.Brokers.Storages.Exceptions
{
    public class FileAlreadyExistsException : Exception
    {
        public FileAlreadyExistsException(string targetPath)
            : base(message: $"The file already exists in the path {targetPath}")
        {

        }
    }
}
