using System.IO;

using AsbaBank.Core;
using AsbaBank.Core.Commands;
using AsbaBank.Core.Messaging;

namespace AsbaBank.Infrastructure
{
    public class MessageBusCommandPublisher : IPublishCommands
    {
        private readonly Messaging.IMessageBus commandService;
        private readonly ISerialize serializer;

        public MessageBusCommandPublisher(ISerialize serializer)
        {
            this.serializer = serializer;
            commandService = new Messaging.MessageBusClient();
        }

        public void Publish(ICommand command)
        {
            Envelope envelope = BuildMessageEnvelope(command);
            commandService.Send(envelope);
        }

        private Envelope BuildMessageEnvelope(ICommand command)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, command);
                var serializedCommand = stream.ToArray();

                return new Envelope
                {
                    Message = serializedCommand
                };
            }
        }
    }
}
