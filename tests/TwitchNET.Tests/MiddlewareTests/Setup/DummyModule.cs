using TwitchNET.Core.Modules;
using TwitchNET.Sample;

namespace TwitchNET.Tests.MiddlewareTests.Setup
{
    public class DummyModule : BaseModule<DummyCommander>
    {
        public void DummyMethode(string param1, int param2)
        {
        }

        public void CustomTypeReaderDummyMethode(TestEnum test)
        {
        }
    }
}