using System;

using AsbaBank.Infrastructure.CommandScripts;

namespace AsbaBank.Presentation.Shell.ApplicationCommands
{
    public class RecordScript : IApplicationCommand
    {
        public string Usage { get { return Key; } }
        public string Key { get { return "RecordScript"; } }

        public void Execute(string[] args)
        {
            ScriptRecorder recorder = Environment.GetScriptRecorder();
            recorder.BeginRecording();
            Console.WriteLine("Recording...");
        }
    }
}