namespace TwitchWrapper.Core.Responses
{
    internal class ResponseModel
    {
        public string UserName { get; set; }
        
        public string Message { get; set; }
        
        public ResponseType ResponseType { get; set; }
    }
}