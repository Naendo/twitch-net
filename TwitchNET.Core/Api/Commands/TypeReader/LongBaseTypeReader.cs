using TwitchNET.Core.Exceptions.TypeReaderException;

namespace TwitchNET.Modules
{
    public sealed class LongBaseTypeReader : BaseTypeReader
    {
        public override object ConvertTo(string input)
        {
            if (!long.TryParse(input, out var result))
            {
                throw new TypeReaderException(typeof(IntBaseTypeReader), input);
            }

            return result;
        }
    }
}