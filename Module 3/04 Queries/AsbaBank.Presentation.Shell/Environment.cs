using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using AsbaBank.ApplicationService;
using AsbaBank.Core;
using AsbaBank.DataModel;
using AsbaBank.Infrastructure;
using AsbaBank.Infrastructure.CommandScripts;
using AsbaBank.Infrastructure.EntityFramework;
using AsbaBank.Presentation.Shell.ConsoleViews;
using AsbaBank.Presentation.Shell.ShellCommands;
using AsbaBank.Presentation.Shell.SystemCommands;
using AsbaBank.Queries;

namespace AsbaBank.Presentation.Shell
{
    public static class Environment
    {
        public static readonly ILog Logger;
        private static readonly Dictionary<string, ICommandBuilder> CommandBuilders;
        private static readonly Dictionary<string, ISystemCommand> SystemCommands;
        private static readonly Dictionary<string, IConsoleView> ConsoleViews; 
        private static readonly ScriptRecorder ScriptRecorder;
        private static readonly IContextFactory ContextFactory;

        static Environment()
        {
            Logger = new ConsoleWindowLogger();
            CommandBuilders = new Dictionary<string, ICommandBuilder>();
            SystemCommands = new Dictionary<string, ISystemCommand>();
            ConsoleViews = new Dictionary<string, IConsoleView>();
            ScriptRecorder = new ScriptRecorder();
            RegsiterSystemCommands();
            RegsiterCommandBuilders();
            RegisterViews();

            Database.SetInitializer(new AsbaContextInitializer());
            ContextFactory = new ContextFactory<AsbaContext>("AsbaBank");
        }

        public static IEnumerable<ICommandBuilder> GetShellCommands()
        {
            return CommandBuilders.Values;
        }

        public static IEnumerable<ISystemCommand> GetSystemCommands()
        {
            return SystemCommands.Values;
        }

        public static ICommandBuilder GetShellCommand(string command)
        {
            return CommandBuilders[command.ToUpper()];
        }

        private static void RegisterViews()
        {
            RegisterView(new AllClientsView(GetClientQueries()));
            RegisterView(new ClientsWithNameView(GetClientQueries()));
        }

        private static void RegisterView(IConsoleView view)
        {
            ConsoleViews.Add(view.Key.ToUpper(), view);
        }

        private static void RegsiterSystemCommands()
        {
            RegsiterSystemCommand(new RecordScript());
            RegsiterSystemCommand(new SaveScript());
            RegsiterSystemCommand(new RunScript());
            RegsiterSystemCommand(new ListScripts());
        }

        private static void RegsiterSystemCommand(ISystemCommand command)
        {
            SystemCommands.Add(command.Key.ToUpper(), command);
        }

        private static void RegsiterCommandBuilders()
        {
            RegisterCommandBuilder(new RegisterClientBuilder());
            RegisterCommandBuilder(new UpdateClientAddressBuilder());
        }

        private static void RegisterCommandBuilder(ICommandBuilder commandBuilder)
        {
            CommandBuilders.Add(commandBuilder.Key.ToUpper(), commandBuilder);
        }

        public static IPublishCommands GetCommandPublisher()
        {
            var commandPublisher = new LocalCommandPublisher();
            var unitOfWork = new EntityFrameworkUnitOfWork(ContextFactory);

            commandPublisher.Subscribe(new ClientService(unitOfWork, Logger));

            return commandPublisher;
        }

        public static ScriptPlayer GetScriptPlayer()
        {
            return new ScriptPlayer(GetCommandPublisher());
        }

        public static ScriptRecorder GetScriptRecorder()
        {
            return ScriptRecorder;
        }

        public static bool IsSystemCommand(string command)
        {
            return SystemCommands.ContainsKey(command.ToUpper());
        }

        public static ISystemCommand GetSystemCommand(string command)
        {
            return SystemCommands[command.ToUpper()];
        }

        public static ClientQueries GetClientQueries()
        {
            var efQuery = GetEntityFrameworkQuery();
            return new ClientQueries(efQuery, efQuery);
        }

        private static EntityFrameworkQuery GetEntityFrameworkQuery()
        {
            return new EntityFrameworkQuery(new ContextFactory<AsbaContext>("AsbaBank"));
        }

        public static IEnumerable<IConsoleView> GetViews()
        {
            return ConsoleViews.Values;
        }

        public static bool IsView(string command)
        {
            return ConsoleViews.ContainsKey(command.ToUpper());
        }

        public static IConsoleView GetView(string request)
        {
            return ConsoleViews[request.ToUpper()];
        }
    }
}