using System.Threading.Tasks;

namespace TwitchNET.Core.PubSub
{
    public delegate Task ReceivePubSubDelegate(string response);
}