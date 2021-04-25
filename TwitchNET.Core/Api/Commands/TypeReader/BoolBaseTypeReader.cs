using TwitchNET.Core.Exceptions.TypeReaderException;

namespace TwitchNET.Modules
{
    public class BoolBaseTypeReader : BaseTypeReader
    {
        public override object ConvertTo(string input)
        {
            if (!bool.TryParse(input, out var result))
                throw new TypeReaderException(typeof(BoolBaseTypeReader), input);

            return result;
        }
    }
}