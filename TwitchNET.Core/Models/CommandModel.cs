namespace TwitchNET.Core.Models
{
    public class CommandModel
    {
        public string CommandKey { get; set; } = null!;

        public string[] Parameter { get; set; } = null!;
    }
}