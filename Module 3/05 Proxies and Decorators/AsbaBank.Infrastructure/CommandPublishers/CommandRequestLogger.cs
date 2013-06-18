using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsbaBank.Core;
using AsbaBank.Core.Commands;

namespace AsbaBank.Infrastructure.CommandPublishers
{
    public class CommandRequestLogger : IPublishCommands
    {
        private readonly IPublishCommands publisher;
        private readonly ICurrentUserSession currentUser;
        private readonly Type requestLoggerType;
        //bad
        private readonly HashSet<object> handlers = new HashSet<object>();

        public CommandRequestLogger(IPublishCommands publisher, ICurrentUserSession currentUser, Type requestLoggerType/* bad way of doing this*/)
        {
            this.publisher = publisher;
            this.currentUser = currentUser;
            this.requestLoggerType = requestLoggerType;
        }

        public void Publish(ICommand command)
        {
            var commandRequest = Attribute.GetCustomAttributes(command.GetType())
              .FirstOrDefault(a => a is CommandRequestAttribute) as CommandRequestAttribute;

            if (commandRequest == null)
            {
                publisher.Publish(command);
            }
            else
            {
                var newCommand = Activator.CreateInstance(requestLoggerType,
                                                                      new object[]
                                                                          {
                                                                              command, command.GetType().ToString(),
                                                                              currentUser.Identity.Name
                                                                          });
                Type handlerGenericType = typeof(IHandleCommand<>);
                Type handlerType = handlerGenericType.MakeGenericType(new[] { command.GetType() });
                object handler = handlers.Single(handlerType.IsInstanceOfType);

                ((dynamic)handler).Execute((dynamic)newCommand);
            }
        }

        public void Subscribe(object handler)
        {
            //bad
            handlers.Add(handler);
            publisher.Subscribe(handler);
        }
    }
}
