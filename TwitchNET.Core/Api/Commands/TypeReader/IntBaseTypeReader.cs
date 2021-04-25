using TwitchNET.Core.Exceptions.TypeReaderException;

namespace TwitchNET.Modules
{
    public sealed class IntBaseTypeReader : BaseTypeReader
    {
        
        public override object ConvertTo(string input)
        {
            if (!int.TryParse(input, out var result))
            {
                throw new TypeReaderException(typeof(IntBaseTypeReader), input);
            }
            
            return result;
        }
    }
}