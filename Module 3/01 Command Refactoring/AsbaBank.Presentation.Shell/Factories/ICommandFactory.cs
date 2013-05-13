using System.Collections.Generic;

namespace AsbaBank.Presentation.Shell.Factories
{
    public interface ICommandFactory
    {
        ICommand BuildCommand(string[] split);
        IEnumerable<IShellCommand> GetShellCommands();
    }
}