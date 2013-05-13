using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsbaBank.Presentation.Shell.Commands
{
    public class NullCommandShell : IShellCommand
    {
        public string Usage { get { return "Command not found."; }  }
        public string Key { get { return "NullCommand"; } }
        public ICommand Build(string[] args)
        {
            return new NullCommand(args[0]);
        }
    }
}
