namespace TwitchWrapper.Core.Commands
{
    public class ConnectedCommand : ICommand
    {
        /*private readonly string _message;
        private readonly string _user;*/

        //:tmi.twitch.tv 001 <user> :Welcome, GLHF!
        public ConnectedCommand(string message)
        {
           /*_message = message[(message.IndexOf(':', 1) + 1)..];
            _user = message[(message.IndexOf(' ', ":tmi.twitch.tv".Length)..(message.IndexOf(':') - 1))];*/
        }


        public string Parse()
        {
            return "Authenticated";
        }
    }
}