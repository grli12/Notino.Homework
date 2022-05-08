namespace Homework.Adapters.Resolvers
{
    public interface IConvertAdapterResolver
    {
        IConvertAdapter Resolve(string key);
    }
}
