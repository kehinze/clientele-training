﻿using System;
using System.Linq;
using System.Threading;

using AsbaBank.Core;
using AsbaBank.Core.Commands;
using AsbaBank.DataModel;
using AsbaBank.Infrastructure.CommandScripts;

namespace AsbaBank.Presentation.Shell
{
    class Program
    {
        static readonly ScriptRecorder Recorder = new ScriptRecorder();
        static readonly ConsoleColor DefaultColor = Console.ForegroundColor;

        static void Main()
        {
            Console.WindowHeight = 40;
            Console.WindowWidth = 120;
            PrintHelp();

            while (true)
            {
                Console.ForegroundColor = DefaultColor;
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
            var request = split.First();
            var parameters = split.Skip(1).ToArray();

            if (Environment.IsSystemCommand(request))
            {
                var command = Environment.GetSystemCommand(request);
                command.Execute(parameters);
            }
            else if (Environment.IsView(request))
            {
                var view = Environment.GetView(request);
                view.Print(parameters);
            }
            else
            {
                ICommandBuilder commandBuilder = Environment.GetShellCommand(request);
                ICommand command = commandBuilder.Build(parameters);

                Recorder.AddCommand(command);

                IPublishCommands commandPublisher = Environment.GetCommandPublisher();
                commandPublisher.Publish(command);
            }
        }

        private static void PrintHelp()
        {
            Console.ForegroundColor = ConsoleColor.Green;
           
            Console.WriteLine("Available commands:");

            foreach (var shellCommand in Environment.GetShellCommands())
            {
                Console.WriteLine("- {0}", shellCommand.Usage);
            }

            Console.WriteLine();
            Console.WriteLine("System commands:");

            foreach (var systemCommand in Environment.GetSystemCommands())
            {
                Console.WriteLine("- {0}", systemCommand.Usage);
            }

            Console.WriteLine();
            Console.WriteLine("Views:");

            foreach (var view in Environment.GetViews())
            {
                Console.WriteLine("- {0}", view.Usage);
            }

            Console.WriteLine();
            Console.WriteLine();
        }
    }    
}
