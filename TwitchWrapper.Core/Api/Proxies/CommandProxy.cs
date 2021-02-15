namespace TwitchWrapper.Core.Proxies
{
    public class CommandProxy
    {
        public string Message { get; set; }


        private bool HasParameter => Message.Contains(' ');
        public string Command => HasParameter ? Message[1..Message.IndexOf(' ')] : Message;
        public string? Parameter => HasParameter ? Message[(Message.IndexOf(' ') + 1)..] : null;
    }
}