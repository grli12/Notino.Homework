using Homework.Adapters.Resolvers.Exceptions;

namespace Homework.Adapters.Resolvers
{
    public class ConvertAdapterResolver : IConvertAdapterResolver
    {
        private readonly IDictionary<string, Type> adapterPool;
        private readonly IServiceProvider serviceProvider;

        public ConvertAdapterResolver(IServiceProvider serviceProvider, Dictionary<string, Type> adapterPool)
        {
            this.adapterPool = adapterPool;
            this.serviceProvider = serviceProvider;
        }

        public IConvertAdapter Resolve(string key)
        {
            if(adapterPool.ContainsKey(key))
            {
                Type type = adapterPool[key];
                var converter = this.serviceProvider.GetServices<IConvertAdapter>().First(t => t.GetType() == type);
                return converter!;
            }

            throw new ConvertAdapterNotFoundException(key);
        }
    }
}
