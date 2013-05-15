namespace AsbaBank.Presentation.Shell
{
    public interface IApplicationCommand
    {
        string Usage { get; }
        string Key { get; }
        void Execute(string[] args);
    }
}