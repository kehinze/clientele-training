using AsbaBank.Core.Commands;

namespace AsbaBank.Core
{
    public interface IPublishCommands
    {
        void Publish<TCommand>(TCommand command) where TCommand : ICommand;
    }
}