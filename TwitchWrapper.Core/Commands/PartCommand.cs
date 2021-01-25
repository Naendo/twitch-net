namespace TwitchWrapper.Core.Models
{
    public class PartCommand : ICommand
    {
        private readonly string _userName;

        public PartCommand(string userName)
        {
            _userName = userName;
        }

        //[PART](#part)
        public string Parse()
        {
            return $"PART #{_userName}";
        }
    }
}