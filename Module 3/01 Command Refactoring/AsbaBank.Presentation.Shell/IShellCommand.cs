namespace AsbaBank.Presentation.Shell
{
    public interface IShellCommand
    {
        string Usage { get; }
        string Key { get; }
        ICommand Build(string[] args);
    }
}