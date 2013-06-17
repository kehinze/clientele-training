﻿using System;
using System.Linq;
using System.Threading;

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
            if (Environment.IsSystemCommand(split.First()))
            {
                var command = Environment.GetSystemCommand(split.First());
                command.Execute(split.Skip(1).ToArray());
            }
            else
            {
                IShellCommand shellCommand = Environment.GetShellCommand(split.First());
                shellCommand.Execute(split.Skip(1).ToArray());
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

            Console.ForegroundColor = originalColor;
            Console.WriteLine();
            Console.WriteLine();
        }
    }    
}
