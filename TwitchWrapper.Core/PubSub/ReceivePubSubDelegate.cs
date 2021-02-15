using System.Threading.Tasks;

namespace TwitchWrapper.Core.PubSub
{
    public delegate Task ReceivePubSubDelegate(string response);
}