using System.Collections.Generic;

using AsbaBank.ApplicationService.CommandHandlers;
using AsbaBank.Core;
using AsbaBank.Infrastructure;
using AsbaBank.Presentation.Shell.ShellCommands;

namespace AsbaBank.Presentation.Shell
{
    public static class Environment
    {
        private static readonly InMemoryDataStore DataStore;
        public static readonly ILog Logger;
        private static readonly Dictionary<string, IShellCommand> ShellCommands; 
        
        static Environment()
        {
            DataStore = new InMemoryDataStore();
            Logger = new ConsoleWindowLogger();
            ShellCommands = new Dictionary<string, IShellCommand>();
            RegisterCommands();
        }

        public static IEnumerable<IShellCommand> GetShellCommands()
        {
            return ShellCommands.Values;
        }

        public static IShellCommand GetShellCommand(string key)
        {
            return ShellCommands[key];
        }

        private static void RegisterCommands()
        {
            RegsiterCommand(new RegisterClientShell());   
        }

        private static void RegsiterCommand(IShellCommand command)
        {
            ShellCommands.Add(command.Key, command);
        }

        public static IPublishCommands GetCommandPublisher()
        {
            var commandPublisher = new LocalCommandPublisher();
            var unitOfWork = new InMemoryUnitOfWork(DataStore);

            commandPublisher.Subscribe(new RegisterClientHandler(unitOfWork, Logger));

            return commandPublisher;
        }
    }
}