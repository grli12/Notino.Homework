namespace Homework.Brokers.Storages
{
    public interface IStorageBroker
    {
        string ReadTextFromFile(string sourcePath);
        string WriteTextToFile(string text, string targetPath);
    }
}
