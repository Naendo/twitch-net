namespace TwitchNET.Core.Models
{
    /// <inheritdoc cref="ICommand"/>
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