using TwitchNET.Core.Commands;

namespace TwitchNET.Core
{
    /// <summary>
    /// Commands are used to parse incoming irc-requests and form usable modules for the system.
    /// </summary>
    /// <example>
    /// For example, following string will get converted to a <see cref="MessageCommand"/>
    /// <code>PRIVMSG #&ltchannel&rt :This is a sample message
    /// :&ltuser&rt!&ltuser&rt@&ltuser&rt.tmi.twitch.tv PRIVMSG #&ltchannel&rt :This is a sample message
    /// </code>
    /// 
    /// </example>
    internal interface ICommand
    {
        string Parse();
    }
}