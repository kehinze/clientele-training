using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsbaBank.Core.Commands;
using AsbaBank.Core;

namespace AsbaBank.Infrastructure.CommandPublishers
{
    public class CommandPublisherProxy : LocalFileCommandPublisher
    {
        public CommandPublisherProxy() : base()
        {
            
        }
        
        public void Publish(ICommand command)
        {
            Console.WriteLine("Hello from Proxy publisher....");
            base.Publish(command);
            Console.WriteLine("End hello from proxy publisher");
        }

        public void Subscribe(object handler)
        {
            Console.WriteLine("Hello from Subscribe....");
            base.Subscribe(handler);
            Console.WriteLine("End hello from Subscribe");
        }
    }
}
