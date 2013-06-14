using System;
using System.Linq;
using AsbaBank.Core;
using AsbaBank.Core.Commands;

namespace AsbaBank.Infrastructure.CommandPublishers
{
    public class RetryPublisher : IPublishCommands
    {
        private readonly IPublishCommands publisher;
        private readonly IUnitOfWork unitOfWork;

        public RetryPublisher(IPublishCommands publisher, IUnitOfWork unitOfWork)
        {
            this.publisher = publisher;
            this.unitOfWork = unitOfWork;
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
                var publish = new Action(() => publisher.Publish(command));
                var rollback = new Action<Exception>(e => unitOfWork.Rollback());

                Retry.Action(publish, rollback, attribute.RetryCount, attribute.RetryMilliseconds);
            }
        }

        public void Subscribe(object handler)
        {
            publisher.Subscribe(handler);                             
        }
    }
}