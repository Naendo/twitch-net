using System.Threading.Tasks;
using TwitchNET.Core.Responses;

namespace TwitchNET.Core
{
    internal delegate Task OnReceivedDelegate(IResponse command);
}