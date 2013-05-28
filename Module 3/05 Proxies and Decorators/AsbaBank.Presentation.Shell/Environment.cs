using System.Collections.Generic;
using System.Data.Entity;
using AsbaBank.ApplicationService;
using AsbaBank.Core;
using AsbaBank.DataModel;
using AsbaBank.Infrastructure;
using AsbaBank.Infrastructure.CommandPublishers;
using AsbaBank.Infrastructure.CommandScripts;
using AsbaBank.Infrastructure.EntityFramework;
using AsbaBank.Presentation.Shell.ShellCommands;
using AsbaBank.Presentation.Shell.SystemCommands;

namespace AsbaBank.Presentation.Shell
{
    public static class Environment
    {
        private static readonly Dictionary<string, ICommandBuilder> CommandBuilders;
        private static readonly Dictionary<string, ISystemCommand> SystemCommands;
        private static readonly ScriptRecorder ScriptRecorder;
        private static readonly IContextFactory ContextFactory;

        static Environment()
        {
            CommandBuilders = new Dictionary<string, ICommandBuilder>();
            SystemCommands = new Dictionary<string, ISystemCommand>();
            ScriptRecorder = new ScriptRecorder();
            RegsiterSystemCommands();
            RegsiterCommandBuilders();

            
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
            var commandPublisher = new CommandPublisherAuthorizerProxy(GetCurrentUserSession());
            var unitOfWork = new EntityFrameworkUnitOfWork(ContextFactory);
            commandPublisher.Subscribe(new ClientService(unitOfWork));

            return new CommandPublisherLoggerDecorator(commandPublisher);
        }

        public static ICurrentUserSession GetCurrentUserSession()
        {
            return new CurrentUserSession
            {
                Role = "Guest",
                UserName = "Clientele\\AFreemantle"
            };
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
    }
}