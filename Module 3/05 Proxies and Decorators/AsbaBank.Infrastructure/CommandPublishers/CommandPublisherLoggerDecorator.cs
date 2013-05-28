using AsbaBank.Core;
using AsbaBank.Core.Commands;
using AsbaBank.Infrastructure.Logging;

namespace AsbaBank.Infrastructure.CommandPublishers
{
    public class CommandPublisherLoggerDecorator : IPublishCommands
    {
        private readonly IPublishCommands publisher;
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof (CommandPublisherLoggerDecorator));

        public CommandPublisherLoggerDecorator(IPublishCommands publisher)
        {
            this.publisher = publisher;
        }

        public void Publish(ICommand command)
        {
            Logger.Verbose("Publishing command {0}", command.GetType());
            publisher.Publish(command);
            Logger.Verbose("Completed publishing command {0}", command.GetType());
        }
    }
}