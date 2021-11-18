using System.Linq;

namespace TwitchNET.Sample.Services
{
    public class TwitchService
    {
        public string TransformResponse(string value)
        {
            return new string(value.Reverse().ToArray());
        }
    }
}