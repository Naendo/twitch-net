namespace TwitchWrapper.Core.Proxies
{
    public class CommandProxy
    {
        public string Message { get; set; }

        public string Command =>
            Message[0..Message.IndexOf(' ')];

        public string Parameter => Message[(Message.IndexOf(' ') + 1)..];

        private bool HasParameter => Command.Contains(' ');
    }
}