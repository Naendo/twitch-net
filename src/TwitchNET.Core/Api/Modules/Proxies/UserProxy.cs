namespace TwitchNET.Core.Modules
{
    public class UserProxy
    {
        public bool IsModerator { get; internal set; }
        public bool IsBroadcaster { get; internal set; }
        public bool IsSubscriber { get; internal set; }

        public bool IsTurbo { get; internal set; }
        public bool IsVip { get; internal set; }

        public string Name { get; internal set; }

        /// <summary>
        ///     Color in Hexformat #FFFFFF
        /// </summary>
        public string Color { get; internal set; }
    }
}