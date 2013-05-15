using System;
using System.Linq;
using System.Threading;

using AsbaBank.Core;
using AsbaBank.Core.Commands;
using AsbaBank.Infrastructure.CommandScripts;

namespace AsbaBank.Presentation.Shell
{
    class Program
    {
        static readonly ScriptRecorder Recorder = new ScriptRecorder();

        static void Main()
        {

            Console.WindowHeight = 40;
            Console.WindowWidth = 120;

            PrintHelp();

            while (true)
            {
                Thread.Sleep(300);
                Console.Write("> ");

                var line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var split = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                TryHandleRequest(split);

                Console.WriteLine();
            }
        }

        private static void TryHandleRequest(string[] split)
        {
            try
            {
                HandleRequest(split);
            }
            catch (Exception ex)
            {
                Environment.Logger.Fatal(ex.Message);
            }
        }

        private static void HandleRequest(string[] split)
        {
            if (Environment.IsSystemCommand(split.First()))
            {
                var command = Environment.GetSystemCommand(split.First());
                command.Execute(split.Skip(1).ToArray());
            }
            else
            {
                ICommandBuilder commandBuilder = Environment.GetShellCommand(split.First());
                ICommand command = commandBuilder.Build(split.Skip(1).ToArray());

                Recorder.AddCommand(command);

                IPublishCommands commandPublisher = Environment.GetCommandPublisher();
                commandPublisher.Publish(command);
            }
        }

        private static void PrintHelp()
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
           
            Console.WriteLine("Available commands:");

            foreach (var shellCommand in Environment.GetShellCommands())
            {
                Console.WriteLine(shellCommand.Usage);
            }

            Console.WriteLine();
            Console.WriteLine("System commands:");

            foreach (var systemCommand in Environment.GetSystemCommands())
            {
                Console.WriteLine(systemCommand.Usage);
            }

            Console.ForegroundColor = originalColor;
            Console.WriteLine();
            Console.WriteLine();
        }
    }    
}
