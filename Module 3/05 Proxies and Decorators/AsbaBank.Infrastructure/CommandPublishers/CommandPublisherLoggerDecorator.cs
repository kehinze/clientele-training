using System;
using System.Collections.Generic;
using System.Linq;
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
            Logger.Verbose("Publishing command {0}", command.GetType().Name);
            publisher.Publish(command);
            Logger.Verbose("Completed publishing command {0}", command.GetType().Name);
        }

        public void Subscribe(object handler)
        {
            publisher.Subscribe(handler);

            foreach (var type in GetCommandHandlerTypes(handler))
            {
                Logger.Verbose("Subscribed handler for command {0}", type.GenericTypeArguments[0].Name);    
            }
        }

        private static IEnumerable<Type> GetCommandHandlerTypes(object handler)
        {
            return handler.GetType()
                          .GetInterfaces()
                          .Where(x =>
                                 x.IsGenericType &&
                                 x.GetGenericTypeDefinition() == typeof (IHandleCommand<>));
        }
    }
}