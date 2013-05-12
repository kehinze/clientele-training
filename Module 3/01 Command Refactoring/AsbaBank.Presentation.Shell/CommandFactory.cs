using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsbaBank.Presentation.Shell.Commands;

namespace AsbaBank.Presentation.Shell
{
    public class CommandFactory
    {
        private static Dictionary<string, IShellCommand> ShellCommands; 

        public CommandFactory()
        {
            ShellCommands = new Dictionary<string, IShellCommand>();
        }

        private static void RegisterCommands()
        {
            RegsiterCommand(new RegisterClientShell());
        }

        public static IEnumerable<IShellCommand> GetShellCommands()
        {
            return ShellCommands.Values;
        }

        public static IShellCommand GetShellCommand(string key)
        {
            return ShellCommands[key];
        }

        private static void RegsiterCommand(IShellCommand command)
        {
            ShellCommands.Add(command.Key, command);
        }
    }



}
