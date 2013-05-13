using System.Collections.Generic;
using System.Linq;
using AsbaBank.Presentation.Shell.Commands;

namespace AsbaBank.Presentation.Shell.Factories
{
    public class CommandFactory : ICommandFactory
    {
        private static Dictionary<string, IShellCommand> shellCommands; 

        public CommandFactory()
        {
            shellCommands = new Dictionary<string, IShellCommand>();
            RegisterCommands();
        }

        //Use reflection to register all commands......
        private void RegisterCommands()
        {
            RegisterCommand(new RegisterClientShell());
            RegisterCommand(new DebitAccountShell());
        }

        public ICommand BuildCommand(string[] split)
        {
            IShellCommand shellCommand = GetShellCommand(split.First());
            return shellCommand.Build(split.Skip(1).ToArray());
        }

        public IEnumerable<IShellCommand> GetShellCommands()
        {
            return shellCommands.Values;
        }

        private IShellCommand GetShellCommand(string key)
        {
            if (shellCommands.ContainsKey(key))
            {
                return shellCommands[key];
            }

            return new NullCommandShell();
        }
        
        private void RegisterCommand(IShellCommand command)
        {
            shellCommands.Add(command.Key, command);
        }
    }



}
