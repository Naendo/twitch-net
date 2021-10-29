using System.Linq;

namespace TwitchNET.Sample.Services
{
    public class DummyService
    {
        public string TransformResponse(string value)
        {
            return new string(value.Reverse().ToArray());
        }
    }
}