using System.Threading.Tasks;
using TwitchWrapper.Core.Responses;

namespace TwitchWrapper.Core
{
    internal delegate Task OnReceivedDelegate(IResponse command);

}