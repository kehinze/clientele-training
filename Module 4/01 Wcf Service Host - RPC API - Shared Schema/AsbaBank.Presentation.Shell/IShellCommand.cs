namespace AsbaBank.Presentation.Shell
{
    public interface IShellCommand
    {
        string Usage { get; }
        string Key { get; }
        void Execute(string[] args);
    }
}