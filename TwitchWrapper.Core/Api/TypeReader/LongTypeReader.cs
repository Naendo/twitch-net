using TwitchWrapper.Core.Exceptions.TypeReaderException;

namespace TwitchWrapper.Core.TypeReader
{
    public sealed class LongTypeReader : TypeReader<long>
    {
        public override long ConvertTo(string input)
        {
            if (!long.TryParse(input, out var result))
            {
                throw new TypeReaderException(typeof(IntTypeReader), input);
            }

            return result;
        }
    }
}