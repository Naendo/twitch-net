using System.Threading.Tasks;

namespace TwitchWrapper.Core.TypeReader
{
    public abstract class TypeReader<TType>
    {
        public abstract TType ConvertTo(string input);
    }
}