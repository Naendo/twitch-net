namespace TwitchWrapper.Core.Proxies
{
    public class CommandProxy
    {
        public string Message { get; set; }
        private string[] Parameter => Message.Split(' ');
        private bool HasParameter => Parameter.Length > 1;
    }
}