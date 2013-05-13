using AsbaBank.Infrastructure.Interfaces;
using AsbaBank.Presentation.Shell.Startup;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsbaBank.Presentation.Shell.Commands
{
    public class NullCommand : ICommand
    {
        private readonly string commandName;

        public NullCommand(string commandName)
        {
            this.commandName = commandName;
        }

        public void Execute()
        {
            using (var kernel = new StandardKernel(new AbsaBankModules()))
            {
                var logger = kernel.Get<ILog>();

                logger.Info(string.Format("The command you have entered does not exist. Command name: {0}", commandName));
            }
        }
    }
}
