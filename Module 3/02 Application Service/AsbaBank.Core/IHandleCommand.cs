namespace AsbaBank.Core
{
    public interface IHandleCommand<in TCommand> where TCommand : ICommand
    {
        void Execute(TCommand command);
    }
}