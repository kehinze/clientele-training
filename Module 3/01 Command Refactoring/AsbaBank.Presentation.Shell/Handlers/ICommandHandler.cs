namespace AsbaBank.Presentation.Shell.Handlers
{
    public interface ICommandHandler
    {
        void TryHandleRequest(string[] split);
    }
}