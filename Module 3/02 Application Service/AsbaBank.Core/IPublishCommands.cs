namespace AsbaBank.Core
{
    public interface IPublishCommands
    {
        void Publish(ICommand command);
    }
}