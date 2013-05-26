using System;
using System.Collections.Generic;
using System.Linq;

using AsbaBank.Core;
using AsbaBank.Core.Commands;

namespace AsbaBank.Infrastructure
{
    public sealed class CommandPublisher : IPublishCommands
    {
        private readonly HashSet<object> handlers;

        public CommandPublisher()
        {
            handlers = new HashSet<object>();
        }

        public void Subscribe(object handler)
        {
            handlers.Add(handler);
        }

        public void Publish<TCommand>(TCommand command) where TCommand : ICommand
        {
            Type handlerGenericType = typeof(IHandleCommand<>);
            Type handlerType = handlerGenericType.MakeGenericType(new[] { command.GetType() });
            object handler = handlers.Single(handlerType.IsInstanceOfType);

            ((dynamic)handler).Execute((dynamic)command);
        }
    }
}
