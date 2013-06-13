using System;
using System.Collections.Generic;
using AsbaBank.Core.Commands;
using AsbaBank.Presentation.Shell.CommandHandlerServices;

namespace AsbaBank.Presentation.Shell.RemoteProxies
{
    public sealed class CommandHandlerProxy<TCommand> : IHandleCommand<TCommand> where TCommand : ICommand
    {
        public void Execute(TCommand command)
        {
            var headers = new Dictionary<string, object>
            {
                {"User", "Clientele\\AFreemantle"},
                {"CorrolationId", Guid.NewGuid()}
            };

            var envelope = new MessageEnvelope
            {
                Command = command,
                Headers = headers
            };
           
            using (var service = new CommandHandlerServiceClient())
            {
                service.Execute(envelope);
            }
        }
    }
}
