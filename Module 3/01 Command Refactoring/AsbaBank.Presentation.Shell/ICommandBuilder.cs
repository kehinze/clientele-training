namespace AsbaBank.Presentation.Shell
{
    public interface ICommandBuilder
    {
        string Usage { get; }
        string Key { get; }
        ICommand Build(string[] args);
    }
}