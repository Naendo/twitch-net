namespace TwitchWrapper.Core.Proxies
{
    public class UserProxy
    {
        public bool IsModerator { get; set; }

        public bool IsBroadcaster { get; set; }

        public bool IsSubscriber { get; set; }

        public bool IsTurbo { get; set; }
        
        public bool IsVip { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Color in Hexformat #FFFFFF
        /// </summary>
        public string Color { get; set; }
        
    }
}