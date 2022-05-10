using Homework.Brokers.Storages.Exceptions;

namespace Homework.Brokers.Storages
{
    public class StorageBroker : IStorageBroker
    {
        public async Task<string> ReadTextFromFileAsync(string sourcePath)
        {
             return await File.ReadAllTextAsync(sourcePath);
        }

        public async Task<string> WriteTextToFileAsync(string text, string targetPath)
        {
            if(File.Exists(targetPath))
            {
                throw new FileAlreadyExistsException(targetPath);
            }

            try
            {
                await File.WriteAllTextAsync(targetPath, text);

                return targetPath;
            }
            catch(Exception innerException)
            {
                throw new StorageFileSaveFailedException(innerException);
            }
        }
    }
}
