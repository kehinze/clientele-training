using System;
using System.Linq;
using AsbaBank.Core;
using AsbaBank.Core.Commands;
using AsbaBank.Infrastructure.Logging;

namespace AsbaBank.Infrastructure.CommandPublishers
{
    public class CommandPublisherAuthorizerProxy : LocalCommandPublisher
    {
        private readonly ICurrentUserSession currentUser;
        private readonly LocalCommandPublisher publisher;
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(CommandPublisherAuthorizerProxy));

        public CommandPublisherAuthorizerProxy(ICurrentUserSession currentUser)
        {
            this.currentUser = currentUser;
            publisher = new LocalCommandPublisher();
        }

        public override void Publish(ICommand command)
        {
            Logger.Verbose("Authorizing command", command.GetType());

            Authorize(command);

            publisher.Publish(command);
        }

        private void Authorize(ICommand command)
        {
            var authorizeAttribute = Attribute.GetCustomAttributes(command.GetType())
                .FirstOrDefault(a => a is CommandAuthorizeAttribute) as CommandAuthorizeAttribute;

            if (authorizeAttribute == null || authorizeAttribute.Role == currentUser.Role)
            {
                return;
            }

            Logger.Error("User {0} with role {1} attempted to execute command {2} which requires role {3}",
                         currentUser.UserName, currentUser.Role, command.GetType().Name, authorizeAttribute.Role);

            throw new Exception("You are not authorized to execute this command.");
        }

        public override void Subscribe(object handler)
        {
            publisher.Subscribe(handler);
        }
    }
}