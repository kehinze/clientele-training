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

            Logger.Verbose("Command was allowed to execute.", command.GetType());
        }

        private void Authorize(ICommand command)
        {
            var authorizeAttribute = Attribute.GetCustomAttributes(command.GetType())
                .FirstOrDefault(a => a is CommandAuthorizeAttribute) as CommandAuthorizeAttribute;

            if (authorizeAttribute == null || currentUser.IsInRole(authorizeAttribute.Roles))
            {
                return;
            }

            Logger.Error("User {0} attempted to execute command {1} which requires roles {2}",
                         currentUser, command.GetType().Name, authorizeAttribute);

            throw new Exception("You are not authorized to execute this command.");
        }

        public override void Subscribe(object handler)
        {
            publisher.Subscribe(handler);
        }
    }
}