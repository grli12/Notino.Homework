using Homework.Adapters;
using Homework.Adapters.Resolvers;
using Homework.Constants;

namespace Homework.Services.Converts
{
    public class ConvertService : IConvertService
    {
        private readonly IConvertAdapterResolver convertAdapterResolver;

        public ConvertService(IConvertAdapterResolver convertAdapterResolver)
        {
            this.convertAdapterResolver = convertAdapterResolver;
        }

        public string Convert(string keyFrom, string keyTo)
        {
            throw new NotImplementedException();
        } 
    }
}
