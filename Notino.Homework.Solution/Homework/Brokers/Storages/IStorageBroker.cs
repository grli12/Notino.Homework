namespace Homework.Brokers.Storages
{
    public interface IStorageBroker
    {
        string ReadTextFromFile(string path);
        string WriteTextToFile(string path);
    }
}
