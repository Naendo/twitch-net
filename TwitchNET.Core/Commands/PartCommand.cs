namespace TwitchNET.Core.Models
{
    internal class PartCommand : ICommand
    {
        private readonly string _userName;

        public PartCommand(string userName)
        {
            _userName = userName;
        }

        //[PART](#part)
        string ICommand.Parse()
        {
            return $"PART #{_userName}";
        }
    }
}