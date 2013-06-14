using System;
using System.Linq;
using AsbaBank.Core;
using AsbaBank.Core.Commands;

namespace AsbaBank.Infrastructure.CommandPublishers
{
    public class CommandPublisherAuthorizer : IPublishCommands
    {
        private readonly ICurrentUserSession currentUser;
        private readonly IPublishCommands publisher;

        public CommandPublisherAuthorizer(IPublishCommands publisher, ICurrentUserSession currentUser)
        {
            this.publisher = publisher;
            this.currentUser = currentUser;
        }

        public void Publish(ICommand command)
        {
            Authorize(command);
            publisher.Publish(command);
        }

        private void Authorize(ICommand command)
        {
            var authorizeAttribute = Attribute.GetCustomAttributes(command.GetType())
                .FirstOrDefault(a => a is CommandAuthorizeAttribute) as CommandAuthorizeAttribute;

            if (authorizeAttribute == null || currentUser.IsInRole(authorizeAttribute.Roles))
            {
                return;
            }

            string message = String.Format("User {0} attempted to execute command {1} which requires roles {2}",
                                           currentUser, command.GetType().Name, authorizeAttribute);

            throw new Exception(message);
        }

        public void Subscribe(object handler)
        {
            publisher.Subscribe(handler);
        }
    }
}