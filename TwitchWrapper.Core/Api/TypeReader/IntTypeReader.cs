using TwitchWrapper.Core.Exceptions.TypeReaderException;

namespace TwitchWrapper.Core.TypeReader
{
    public sealed class IntTypeReader : TypeReader<int>
    {
        public override int ConvertTo(string input)
        {
            if (!int.TryParse(input, out var result))
            {
                throw new TypeReaderException(typeof(IntTypeReader), input);
            }


            return result;
        }
    }
}