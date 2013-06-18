using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AsbaBank.Core.Commands;
using System.Xml;

namespace AsbaBank.Infrastructure.CommandPublishers
{
    /* really badly written */
    public class LocalFileSubscriber
    {
        private readonly string filePath;
        private Thread thread; 
        private readonly List<object> handlers;

        public LocalFileSubscriber(string filePath)
        {
            this.filePath = filePath;
            handlers = new List<object>();
        }

        public void InitializeInstance()
        {
            thread = new Thread(StartSubscribers);
            thread.Start();
        }

        public void AddHandler(object handler)
        {
            handlers.Add(handler);
        }

        private void StartSubscribers()
        {
            while (true)
            {
                ReadFileAndExecuteCommand();
                Thread.Sleep(10);
            }
        }
        
        private void ReadFileAndExecuteCommand()
        {
            if (handlers.Count == 0)
            {
                return;
            }

            ExecuteCommands();
        }
        //method to long
        private void ExecuteCommands()
        {
            var files = Directory.GetFiles(filePath, "*.xml");

            foreach (var file in files)
            {
                string commandName = Path.GetFileName(file.Split('-')[0]);

                var parameters = new List<object>();

                using (XmlTextReader reader = new XmlTextReader(file))
                {
                    reader.Read();

                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(reader);

                    foreach (XmlElement xmlElement in xmlDoc.DocumentElement.ChildNodes)
                    {
                        parameters.Add(xmlElement.InnerText);
                    }
                }

                var data = Activator.CreateInstance(GetPayloadTypeFromCommandName(commandName), parameters.ToArray());

                object handler = GetHandlerForCommand(commandName);

                ((dynamic)handler).Execute((dynamic)data);
            }
        }

        //this could be simplyfied
        private object GetHandlerForCommand(string commandName)
        {
            foreach (var handler in handlers)
            {
                Type handlerType = handler.GetType();

                var allHandlerInterfaces = handlerType.GetInterfaces();

                foreach (var allHandlerInterface in allHandlerInterfaces)
                {
                    var genericTypeDefinition = allHandlerInterface.GetGenericArguments().SingleOrDefault(c => c.Name.Equals(commandName));

                    if (genericTypeDefinition != null)
                    {
                        return handler;
                    }
                }
            }

            throw new InvalidOperationException(string.Format("No handler found for the command {0}", commandName));
        }

        //this could be simplyfied
        public Type GetPayloadTypeFromCommandName(string commandName)
        {
            foreach (var handler in handlers)
            {
                Type handlerType = handler.GetType();

                var allHandlerInterfaces = handlerType.GetInterfaces();

                foreach (var allHandlerInterface in allHandlerInterfaces)
                {
                    var genericTypeDefinition = allHandlerInterface.GetGenericArguments().SingleOrDefault(c => c.Name.Equals(commandName));

                    if (genericTypeDefinition != null)
                    {
                         return genericTypeDefinition;
                    }
                }
            }

            throw new InvalidOperationException(string.Format("No handler found for the command {0}", commandName));
        }
    }


}
