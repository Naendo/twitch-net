using TwitchNET.Core.Exceptions.TypeReaderException;

namespace TwitchNET.Modules
{
    public sealed class NullableIntBaseTypeReader : BaseTypeReader
    {
        public override object ConvertTo(string input)
        {
            if (input.Trim().Length == 0) return null!;

            if (!int.TryParse(input, out var result))
            {
                throw new TypeReaderException(typeof(IntBaseTypeReader), input);
            }
            
            return result;
        }
    }
}