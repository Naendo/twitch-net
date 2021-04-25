using TwitchNET.Core.Exceptions.TypeReaderException;

namespace TwitchNET.Modules
{
    public class DoubleBaseTypeReader : BaseTypeReader
    {
        public override object ConvertTo(string input)
        {
            if (!double.TryParse(input, out var result))
                throw new TypeReaderException(typeof(BoolBaseTypeReader), input);

            return result;
        }
    }
}