using AsbaBank.Core;

namespace AsbaBank.Presentation.Shell
{
    public interface IShellCommand
    {
        string Usage { get; }
        string Key { get; }
        void Build(string[] args);
        void Execute();
    }
}