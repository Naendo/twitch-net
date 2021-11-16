using System.Collections;
using System.Threading.Tasks;
using TwitchNET.Core.Commands;

namespace TwitchNET.Core.Modules
{
    /// <summary>
    /// Provides a base class for a command module to inherit from
    /// </summary>
    public abstract class BaseModule<TCommander> : ModuleProxyBase
        where TCommander : class
    {
    }
}