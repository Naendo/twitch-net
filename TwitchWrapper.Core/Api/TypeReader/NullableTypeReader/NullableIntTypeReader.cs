using TwitchWrapper.Core.Exceptions.TypeReaderException;

namespace TwitchWrapper.Core.TypeReader
{
    public sealed class NullableIntTypeReader : TypeReader<int?>
    {
        public override int? ConvertTo(string input)
        {
            if (input.Trim().Length == 0) return null;

            if (!int.TryParse(input, out var result))
            {
                throw new TypeReaderException(typeof(IntTypeReader), input);
            }


            return result;
        }
    }
}