using BenchmarkDotNet.Attributes;
using TwitchLib.Client;
using TwitchNET.Core;


namespace TwitchNET.Benchmarks.TwitchNET.Setup
{
    public class TwitchClientBenchmark
    {

        [Benchmark]
        public void TwitchClient_TwitchNET()
        {
            var twitchBot = new TwitchBot();
        }

        [Benchmark]
        public void TwitchClient_TwitchLib()
        {
            var twitchbot = new TwitchClient();
        }
        
        
        
        
    }
}