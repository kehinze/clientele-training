using System.Collections.Generic;
using AsbaBank.ApplicationService;
using AsbaBank.Core;
using AsbaBank.Infrastructure;
using AsbaBank.Presentation.Shell.ShellCommands;

namespace AsbaBank.Presentation.Shell
{
    public static class Environment
    {
        private static readonly InMemoryDataStore DataStore;
        public static readonly ILog Logger;
        private static readonly Dictionary<string, IShellCommand> CommandBuilders; 
        
        static Environment()
        {
            DataStore = new InMemoryDataStore();
            Logger = new ConsoleWindowLogger();
            CommandBuilders = new Dictionary<string, IShellCommand>();
            RegsiterShellCommands();
        }

        public static IEnumerable<IShellCommand> GetShellCommands()
        {
            return CommandBuilders.Values;
        }

        public static IShellCommand GetShellCommand(string key)
        {
            return CommandBuilders[key];
        }

        private static void RegsiterShellCommands()
        {
            RegsiterShellCommand(new RegisterClientShellCommand());
            RegsiterShellCommand(new UpdateClientAddressShellCommand());   
        }

        private static void RegsiterShellCommand(IShellCommand shellCommand)
        {
            CommandBuilders.Add(shellCommand.Key, shellCommand);
        }

        public static IClientService GetClientService()
        {
            return new ClientService(new InMemoryUnitOfWork(DataStore), Logger);
        }
    }
}