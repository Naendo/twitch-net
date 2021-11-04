using System.Threading.Tasks;

namespace TwitchNET.Sample
{
    class Program
    {
        static async Task Main(string[] args)
            => await new TwitchController().InitializeAsync();
    }
}