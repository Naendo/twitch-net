using TwitchNET.Core.Modules;

namespace TwitchNET.Tests.MiddlewareTests.Setup
{
    public class DummyModule : BaseModule
    {
        public void DummyMethode(string param1, int param2)
        {
        }


        public void CustomTypeReaderDummyMethode(TestEnum test)
        {
        }
    }
}