using System.Threading.Tasks;
using TwitchNET.Core.Responses;

namespace TwitchNET.Core.Delegates
{
    internal delegate Task OnReceivedDelegate(IResponse command);
    
    internal delegate Task LogAsyncDelegate(string message, bool isException = false);
}