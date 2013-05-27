using AsbaBank.Core.Commands;
using AsbaBank.Presentation.Shell.CommandHandlerServices;

namespace AsbaBank.Presentation.Shell.RemoteProxies
{
    public sealed class CommandHandlerProxy<TCommand> : IHandleCommand<TCommand> where TCommand : ICommand
    {
        public void Execute(TCommand command)
        {
            using (var service = new CommandHandlerServiceClient())
            {
                service.Execute(command);
            }
        }
    }
}
