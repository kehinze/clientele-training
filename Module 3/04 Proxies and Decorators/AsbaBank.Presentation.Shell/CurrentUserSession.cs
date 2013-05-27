namespace AsbaBank.Infrastructure.CommandPublishers
{
    public class CurrentUserSession : ICurrentUserSession
    {
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}