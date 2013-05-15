using System;

using AsbaBank.Infrastructure.CommandScripts;

namespace AsbaBank.Presentation.Shell.ApplicationCommands
{
    public class SaveScript : IApplicationCommand
    {
        public string Usage { get { return String.Format("{0} <ScriptName>", Key); } }
        public string Key { get { return "SaveScript"; } }

        public void Execute(string[] args)
        {
            ScriptRecorder recorder = Environment.GetScriptRecorder();
            recorder.Save(args[0]);
            Console.WriteLine("Script saved.");
        }
    }
}