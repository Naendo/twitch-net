using System;
using TwitchWrapper.Core.Exceptions.TypeReaderException;

namespace TwitchWrapper.Core.TypeReader
{
    public class DoubleTypeReader : TypeReader<double>
    {
        public override double ConvertTo(string input)
        {
            if (!double.TryParse(input, out var result))
                throw new TypeReaderException(typeof(BoolTypeReader), input);

            return result;
        }
    }
}