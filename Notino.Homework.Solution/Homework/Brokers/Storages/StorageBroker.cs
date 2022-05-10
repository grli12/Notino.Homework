using Homework.Brokers.Storages.Exceptions;

namespace Homework.Brokers.Storages
{
    public class StorageBroker : IStorageBroker
    {
        public string ReadTextFromFile(string sourcePath)
        {
             return File.ReadAllText(sourcePath);
        }

        public string WriteTextToFile(string text, string targetPath)
        {
            if(File.Exists(targetPath))
            {
                throw new FileAlreadyExistsException(targetPath);
            }

            try
            {
                File.WriteAllText(targetPath, text);

                return targetPath;
            }
            catch(Exception innerException)
            {
                throw new StorageFileSaveFailedException(innerException);
            }
        }
    }
}
