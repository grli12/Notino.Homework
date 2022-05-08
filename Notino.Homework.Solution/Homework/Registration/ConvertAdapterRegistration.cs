using Homework.Adapters;
using Homework.Adapters.Resolvers;

namespace Homework.Registration
{
    public static class ConvertAdapterRegistration
    {
        private static Dictionary<string, Type>? adapterPool = null;

        public static IServiceCollection UseConvertAdapters(this IServiceCollection services)
        {
            adapterPool = new Dictionary<string, Type>();
            services.AddSingleton(adapterPool!);
            services.AddScoped<IConvertAdapterResolver, ConvertAdapterResolver>();

            return services;
        }

        public static IServiceCollection AddAdapter<T>(this IServiceCollection services, string key)
            where T : class, IConvertAdapter        
        {
            if(adapterPool == null || adapterPool.ContainsKey(key))
            {
                throw new InvalidOperationException();//TODO: nahradit vlastnou vynimkou
            }

            services.AddScoped<IConvertAdapter, T>();
            adapterPool?.Add(key, typeof(T));

            return services;
        }
    }
}
