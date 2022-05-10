namespace Homework.Brokers.Storages
{
    public interface IStorageBroker
    {
        Task<string> ReadTextFromFileAsync(string sourcePath);
        Task<string> WriteTextToFileAsync(string text, string targetPath);
    }
}
