using System.Collections.Generic;
using AsbaBank.Core;
using AsbaBank.Infrastructure;
using AsbaBank.Presentation.Shell.ShellCommands;

namespace AsbaBank.Presentation.Shell
{
    public static class Environment
    {
        public static readonly ILog Logger;
        private static readonly Dictionary<string, IShellCommand> CommandBuilders;
        private static readonly Dictionary<string, ISystemCommand> SystemCommands;

        static Environment()
        {
            Logger = new ConsoleWindowLogger();
            CommandBuilders = new Dictionary<string, IShellCommand>();
            SystemCommands = new Dictionary<string, ISystemCommand>();
            RegsiterCommandBuilders();
        }

        public static IEnumerable<IShellCommand> GetShellCommands()
        {
            return CommandBuilders.Values;
        }

        public static IEnumerable<ISystemCommand> GetSystemCommands()
        {
            return SystemCommands.Values;
        }

        public static IShellCommand GetShellCommand(string command)
        {
            return CommandBuilders[command.ToUpper()];
        }

        private static void RegsiterCommandBuilders()
        {
            RegisterCommandBuilder(new RegisterClientBuilder());
            RegisterCommandBuilder(new UpdateClientAddressBuilder());
        }

        private static void RegisterCommandBuilder(IShellCommand commandBuilder)
        {
            CommandBuilders.Add(commandBuilder.Key.ToUpper(), commandBuilder);
        }
    
        public static bool IsSystemCommand(string command)
        {
            return SystemCommands.ContainsKey(command.ToUpper());
        }

        public static ISystemCommand GetSystemCommand(string command)
        {
            return SystemCommands[command.ToUpper()];
        }
    }
}