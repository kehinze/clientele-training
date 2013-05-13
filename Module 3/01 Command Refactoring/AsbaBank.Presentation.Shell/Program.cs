using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AsbaBank.Infrastructure;
using AsbaBank.Infrastructure.Implementations.Logger;
using AsbaBank.Presentation.Shell.Factories;
using AsbaBank.Infrastructure.Interfaces;
using AsbaBank.Presentation.Shell.Handlers;

namespace AsbaBank.Presentation.Shell
{
    class Program
    {
        private static CommandFactory commandFactory;
        private static ILog log;
        private static ICommandHandler commandHandler;

        static void Main()
        {
            commandFactory = new CommandFactory();
            log = new ConsoleWindowLogger();
            commandHandler = new CommandHandler(commandFactory, log);

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

                commandHandler.TryHandleRequest(split);

                Console.WriteLine();
            }
        }
        
        private static void PrintHelp()
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("Available commands:");

            foreach (var shellCommand in commandFactory.GetShellCommands())
            {
                Console.WriteLine(shellCommand.Usage);
            }

            Console.ForegroundColor = originalColor;
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
