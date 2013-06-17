using System;
using System.Collections.Generic;
using System.Linq;

using AsbaBank.Core;
using AsbaBank.Core.Commands;

namespace AsbaBank.Infrastructure
{
    /// <summary>
    /// This publisher will publish the command messages to local subscribers, this means we are not crossing process boundaries.
    /// It is however possible to have a command publisher that sends our command messages to a remote process via a 
    /// messaging technology such as WCF, Rabbit MQ, Zer0 MQ, MSMQ, Serivce Bus, nServiceBus etc. This means we can easily combine 
    /// or separate our web and worker roles into a single or distributed process.
    /// </summary>
    public sealed class LocalCommandPublisher : IPublishCommands
    {
        private readonly HashSet<object> handlers;

        public LocalCommandPublisher()
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
