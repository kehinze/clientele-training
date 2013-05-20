using System;

namespace AsbaBank.Presentation.Shell.ApplicationCommands
{
    public class ListScripts : IApplicationCommand
    {
        public string Usage { get { return Key; } }
        public string Key { get { return "Scripts"; } }

        public void Execute(string[] args)
        {
            var scriptPlayer = Environment.GetScriptPlayer();

            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            foreach (var availableScript in scriptPlayer.GetAvailableScripts())
            {
                Console.WriteLine("{0} ", availableScript);
            }

            Console.ForegroundColor = originalColor;
        }
    }
}
