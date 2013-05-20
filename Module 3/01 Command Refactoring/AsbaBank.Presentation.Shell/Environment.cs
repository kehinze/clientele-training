using System.Collections.Generic;
using AsbaBank.Infrastructure;
using AsbaBank.Presentation.Shell.Commands;

namespace AsbaBank.Presentation.Shell
{
    /// <remarks>
    /// A central place for managing our dependencies. In a large and complex
    /// application we may consider using an IoC framework.
    /// </remarks>
    public static class Environment
    {
        private static readonly InMemoryDataStore DataStore;
        public static readonly ILog Logger;
        private static readonly Dictionary<string, ICommandBuilder> CommandBuilders; 
        
        static Environment()
        {
            DataStore = new InMemoryDataStore();
            Logger = new ConsoleWindowLogger();
            CommandBuilders = new Dictionary<string, ICommandBuilder>();
            RegisterCommandBuilders();
        }

        public static IUnitOfWork GetUnitOfWork()
        {
            return new InMemoryUnitOfWork(DataStore);
        }

        public static IEnumerable<ICommandBuilder> GetShellCommands()
        {
            return CommandBuilders.Values;
        }

        public static ICommandBuilder GetShellCommand(string key)
        {
            return CommandBuilders[key];
        }

        private static void RegisterCommandBuilders()
        {
            RegisterCommandBuilder(new RegisterClientBuilder());   
        }

        private static void RegisterCommandBuilder(ICommandBuilder commandBuilder)
        {
            CommandBuilders.Add(commandBuilder.Key, commandBuilder);
        }

    }
}