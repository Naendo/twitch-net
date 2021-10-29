namespace TwitchNET.Core.Modules
{
    public class CommandProxy
    {
        public string Message { get; set; } = null!;
        private string[] Parameter => Message.Split(' ');
        private bool HasParameter => Parameter.Length > 1;
    }
}