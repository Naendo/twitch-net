using System.Linq;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using TwitchNET.Benchmarks.TwitchNET.Setup;

namespace TwitchNET.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ManualConfig();

            config.WithOptions(ConfigOptions.DisableOptimizationsValidator);

            config.AddExporter(DefaultConfig.Instance.GetExporters().ToArray());
            config.AddLogger(DefaultConfig.Instance.GetLoggers().ToArray());
            config.AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());

            BenchmarkRunner.Run<TwitchClientBenchmark>(config);

            //new TwitchClientBenchmark().TwitchClient_TwitchLib();

            //BenchmarkRunner.Run<TwitchClientBenchmark>(config);
        }
    }
}