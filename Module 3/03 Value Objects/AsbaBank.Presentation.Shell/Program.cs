using System;
using System.Linq;
using System.Threading;

using AsbaBank.Core;
using AsbaBank.Core.Commands;

namespace AsbaBank.Presentation.Shell
{
    class Program
    {
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
            IShellCommand shellCommand = Environment.GetShellCommand(split.First());
            ICommand command = shellCommand.Build(split.Skip(1).ToArray());

            IPublishCommands commandPublisher = Environment.GetCommandPublisher();
            commandPublisher.Publish(command);
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

            Console.ForegroundColor = originalColor;
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
