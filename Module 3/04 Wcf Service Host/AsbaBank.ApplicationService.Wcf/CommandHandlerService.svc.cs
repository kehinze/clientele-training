using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

using AsbaBank.Core;
using AsbaBank.Core.Commands;
using AsbaBank.Infrastructure;

namespace AsbaBank.ApplicationService.Wcf
{
    [ServiceContract(Namespace = "Asba.Commands")]
    public class CommandHandlerService
    {
        [OperationContract]
        public void Execute(string command)
        {
            var c = command.DeserializeJson<ICommand>(GetKnownTypes());
            IPublishCommands commandPublisher = Environment.GetCommandPublisher();
            commandPublisher.Publish(c);
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            var coreAssembly = Assembly.Load(new AssemblyName("AsbaBank.Contracts"));

            var commandTypes =
                from type in coreAssembly.GetExportedTypes()
                where typeof(ICommand).IsAssignableFrom(type)
                select type;

            var result = commandTypes.ToArray();

            return result;
        }
    }
}
