using System.IO;

using AsbaBank.Core;
using AsbaBank.Core.Commands;
using AsbaBank.Core.Messaging;

namespace AsbaBank.ApplicationService.Host
{
    public class MessageBus : IMessageBus
    {
        private readonly IPublishCommands commandPublisher = Environment.GetCommandPublisher();
        private readonly ISerialize serializer = Environment.GetSerializer();

        public void Send(Envelope c)
        {
            using (var stream = new MemoryStream(c.Message))
            {
                var command = serializer.Deserialize<ICommand>(stream);
                commandPublisher.Publish(command);
            }
        }
    }
}
