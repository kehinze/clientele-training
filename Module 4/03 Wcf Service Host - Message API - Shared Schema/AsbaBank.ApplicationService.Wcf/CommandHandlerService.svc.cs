using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;

using AsbaBank.Core;
using AsbaBank.Core.Commands;

namespace AsbaBank.ApplicationService.Wcf
{
    [DataContract(Namespace = "Asba.Commands", Name = "MessageEnvelope")]
    public class MessageEnvelope
    {
        [DataMember]
        public Dictionary<string, object> Headers { get; set; }
        [DataMember]
        public object Command { get; set; }

        public MessageEnvelope()
        {
            Headers = new Dictionary<string, object>();
        }
    }

    [ServiceContract(Namespace = "Asba.Commands")]
    [ServiceKnownType("GetKnownTypes")]
    public class CommandHandlerService
    {
        [OperationContract]
        public void Execute(MessageEnvelope message)
        {
            IPublishCommands commandPublisher = Environment.GetCommandPublisher();
            commandPublisher.Publish((dynamic)message.Command);
        }

        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            var coreAssembly = Assembly.Load(new AssemblyName("AsbaBank.ApplicationService"));

            var commandTypes =
                from type in coreAssembly.GetExportedTypes()
                where typeof(ICommand).IsAssignableFrom(type)
                select type;

            var result = commandTypes.ToArray();

            return result;
        }
    }
}
