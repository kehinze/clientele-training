using System;
using System.Collections.Generic;
using System.Linq;
using AsbaBank.Core;
using AsbaBank.Core.Commands;

namespace AsbaBank.Infrastructure.CommandPublishers
{ 
    public class LocalCommandPublisher : IPublishCommands
    {
        private readonly HashSet<object> handlers;

        public LocalCommandPublisher()
        {
            handlers = new HashSet<object>();
        }

        public virtual void Subscribe(object handler)
        {
            handlers.Add(handler);
        }

        public virtual void Publish(ICommand command) 
        {
            Type handlerGenericType = typeof(IHandleCommand<>);
            Type handlerType = handlerGenericType.MakeGenericType(new[] { command.GetType() });
            object handler = handlers.Single(handlerType.IsInstanceOfType);

            ((dynamic)handler).Execute((dynamic)command);
        }
    }
}
