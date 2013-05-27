using System.Collections.Generic;
using AsbaBank.Core;
using AsbaBank.Core.Queries;
using AsbaBank.Infrastructure;
using AsbaBank.Infrastructure.CommandScripts;
using AsbaBank.Presentation.Shell.CommandHandlerServices;
using AsbaBank.Presentation.Shell.QueryServices;
using AsbaBank.Presentation.Shell.RemoteProxies;
using AsbaBank.Presentation.Shell.ShellCommands;
using AsbaBank.Presentation.Shell.SystemCommands;

namespace AsbaBank.Presentation.Shell
{
    public static class Environment
    {
        public static readonly ILog Logger;
        private static readonly Dictionary<string, IShellCommand> CommandBuilders;
        private static readonly Dictionary<string, ISystemCommand> SystemCommands;
        private static readonly ScriptRecorder ScriptRecorder;

        static Environment()
        {
            Logger = new ConsoleWindowLogger();
            CommandBuilders = new Dictionary<string, IShellCommand>();
            SystemCommands = new Dictionary<string, ISystemCommand>();
            ScriptRecorder = new ScriptRecorder();
            RegsiterSystemCommands();
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

        private static void RegisterCommandBuilder(IShellCommand commandBuilder)
        {
            CommandBuilders.Add(commandBuilder.Key.ToUpper(), commandBuilder);
        }

        public static IPublishCommands GetCommandPublisher()
        {
            var commandPublisher = new CommandPublisher();

            commandPublisher.Subscribe(new CommandHandlerProxy<RegisterClient>());
            commandPublisher.Subscribe(new CommandHandlerProxy<UpdateClientAddress>());

            return commandPublisher;
        }

        public static IQueryProcessor GetQueryProcessor()
        {
            var processor = new QueryProcessor();
            processor.Subscribe(new QueryHandlerProxy<FetchAllClients, ClientDto[]>());
            return processor;
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