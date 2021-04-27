using System;

namespace TwitchNET.Modules.TypeReader
{
    /// <summary>
    /// Provides an interface to create customized TypeReader
    /// </summary>
    public interface ITypeReader
    {
        /// <summary>
        /// Returns converted <see cref="string"/> to defined <see cref="Type"/>
        /// </summary>
        /// <param name="type">Defined output type</param>
        /// <param name="input">Twitch Message</param>
        object ConvertFrom(Type type, string input);
    }
}