using System;
using System.Linq;
using AsbaBank.Core;
using AsbaBank.Core.Commands;

namespace AsbaBank.Infrastructure.CommandPublishers
{
    public class CommandRetry : IPublishCommands
    {
        private readonly IPublishCommands publisher;

        public CommandRetry(IPublishCommands publisher)
        {
            this.publisher = publisher;
        }

        public void Publish(ICommand command)
        {
            var attribute = Attribute.GetCustomAttributes(command.GetType())
                                     .FirstOrDefault(a => a is CommandRetryAttribute) as CommandRetryAttribute;

            if (attribute == null)
            {
                publisher.Publish(command);
            }
            else
            {
                var publishAction = new Action(() => publisher.Publish(command));
                Retry.Action(publishAction, attribute.RetryCount, attribute.RetryMilliseconds);
            }
        }

        public void Subscribe(object handler)
        {
            publisher.Subscribe(handler);                             
        }
    }
}