namespace TwitchWrapper.Core
{
    public sealed class UserModel
    {
        public bool IsBroadcaster { get; }
        public bool IsVip { get; }
        public bool IsSubscriber { get; }
        public string Name { get; }
        public string ChatColor { get; }


        public UserModel(bool isBroadcaster, bool isVip, bool isSubscriber, string name, string chatColor)
        {
            IsBroadcaster = isBroadcaster;
            IsVip = isVip;
            IsSubscriber = isSubscriber;
            Name = name;
            ChatColor = chatColor;
        }
    }
}