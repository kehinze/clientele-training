using System.Collections.Generic;
using AsbaBank.Infrastructure;
using AsbaBank.Infrastructure.Implementations.InMemory;
using AsbaBank.Infrastructure.Implementations.Logger;
using AsbaBank.Infrastructure.Interfaces;
using AsbaBank.Presentation.Shell.Commands;

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

        public static IUnitOfWork GetUnitOfWork()
        {
            return new InMemoryUnitOfWork(DataStore);
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

    }
}