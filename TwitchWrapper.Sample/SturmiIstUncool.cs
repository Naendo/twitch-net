using System;
using System.Threading.Tasks;
using TwitchWrapper.Core;
using TwitchWrapper.Core.Attributes;

namespace TwitchWrapper.Sample
{
    public class SturmiIstUncool : BaseModule
    {


        [Command("sturmi")]
        public async Task  SturmiIstSehrUncool()
        {
            await SendAsync("true sturmi stinkt BRUTAL");
        }
        
        
    }
}