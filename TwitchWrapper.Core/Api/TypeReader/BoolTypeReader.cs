using System;
using TwitchWrapper.Core.Exceptions.TypeReaderException;

namespace TwitchWrapper.Core.TypeReader
{
    public class BoolTypeReader : TypeReader<bool>
    {
        public override bool ConvertTo(string input)
        {
            if (!bool.TryParse(input, out var result))
                throw new TypeReaderException(typeof(BoolTypeReader), input);

            return result;
        }
    }
}