namespace AsbaBank.Infrastructure.CommandPublishers
{
    public interface ICurrentUserSession
    {
        string UserName { get; }
        string Role { get; }
    }
}