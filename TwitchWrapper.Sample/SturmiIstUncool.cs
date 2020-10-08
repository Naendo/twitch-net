using System;
using TwitchWrapper.Core;
using TwitchWrapper.Core.Attributes;

namespace TwitchWrapper.Sample
{
    public class SturmiIstUncool : BaseModule
    {


        [Command("sturmi")]
        public void SturmiIstSehrUncool()
        {
            Console.WriteLine("JA TRUE STURMI STINKT WIE DRECK");
        }
        
        
    }
}