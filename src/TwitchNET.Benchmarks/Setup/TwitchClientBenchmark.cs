using BenchmarkDotNet.Attributes;


namespace TwitchNET.Benchmarks.TwitchNET.Setup
{
    public class TwitchClientBenchmark
    {
        private static string _message =
            ":<user>!<user>@<user>.tmi.twitch.tv PRIVMSG #<channel> :This is a sample message @badge-info=subscriber/14;badges=broadcaster/1,subscriber/3012;client-nonce=725174698bf0ca4bc484d300c92494c4;color=#FFFFFF;display-name=ThatNandoTho;emotes=;flags=;id=0227c264-d0c8-4a67-b5a2-dd77bda2c1b0;mod=0;room-id=170382741;subscriber=1;tmi-sent-ts=1602178847063;turbo=0;user-id=170382741;user-type= :thatnandotho!thatnandotho@thatnandotho.tmi.twitch.tv PRIVMSG #thatnandotho :asd";

        [Benchmark]
        public void TwitchClient_TwitchNET()
        {
        }

        [Benchmark]
        public void TwitchClient_TwitchLib()
        {
            
        }
    }
}