using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AsbaBank.Core;
using AsbaBank.Core.Commands;
using System.IO;
using System.Xml;

namespace AsbaBank.Infrastructure.CommandPublishers
{
    public class LocalFileCommandPublisher : IPublishCommands
    {
        private string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\Queue\\";
        private LocalFileSubscriber localFileSubscriber;
        
        public LocalFileCommandPublisher()
        {
            localFileSubscriber = new LocalFileSubscriber(filePath);
            localFileSubscriber.InitializeInstance();
        }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        public void Publish(ICommand command)
        {
            var xmlDocument = new XmlDocument();
            var  xmlnoderoot = xmlDocument.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xmlDocument.AppendChild(xmlnoderoot);

            using (XmlWriter writer = XmlWriter.Create(filePath + command.GetType().Name + "-" + Guid.NewGuid() + ".xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement(command.GetType().Name);

                foreach (var property in command.GetType().GetProperties())
                {
                    writer.WriteElementString(property.Name, GetPropValue(command, property.Name).ToString());
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public void Subscribe(object handler)
        {
            localFileSubscriber.AddHandler(handler);
        }
    }
}
