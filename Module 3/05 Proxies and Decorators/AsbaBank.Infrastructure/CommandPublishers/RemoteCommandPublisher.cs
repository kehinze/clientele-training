using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsbaBank.Core;
using AsbaBank.Core.Commands;

namespace AsbaBank.Infrastructure.CommandPublishers
{
    public class RemoteCommandPublisher : IPublishCommands
    {
        private readonly HashSet<object> handlers;

        public RemoteCommandPublisher()
        {
            handlers = new HashSet<object>();
        }

        public void Subscribe(object handler)
        {
            handlers.Add(handler);
        }

        public void Publish(ICommand command) 
        {
            Type handlerGenericType = typeof(IHandleCommand<>);
            Type handlerType = handlerGenericType.MakeGenericType(new[] { command.GetType() });
            object handler = handlers.Single(handlerType.IsInstanceOfType);

            ((dynamic)handler).Execute((dynamic)command);
        }
    }
}
