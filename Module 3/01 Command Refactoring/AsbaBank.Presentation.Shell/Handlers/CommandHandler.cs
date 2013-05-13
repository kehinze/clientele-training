using AsbaBank.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsbaBank.Presentation.Shell.Factories;

namespace AsbaBank.Presentation.Shell.Handlers
{
    public class CommandHandler : ICommandHandler
    {
        private readonly ICommandFactory commandFactory;
        private readonly ILog log;

        public CommandHandler(ICommandFactory commandFactory, ILog log)
        {
            this.commandFactory = commandFactory;
            this.log = log;
        }

        public void TryHandleRequest(string[] split)
        {
            try
            {
                HandleRequest(split);
            }
            catch (Exception ex)
            {
                log.Fatal(ex.Message);
            }
        }

        private void HandleRequest(string[] split)
        {
            ICommand command = commandFactory.BuildCommand(split);

            command.Execute();
        }
    }
}
