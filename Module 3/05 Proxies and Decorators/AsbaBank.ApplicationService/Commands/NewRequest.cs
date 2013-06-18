using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using AsbaBank.Core.Commands;
using AsbaBank.Domain.Models;

namespace AsbaBank.ApplicationService.Commands
{
    [DataContract]
    [CommandRetry(RetryCount = 3, RetryMilliseconds = 1000)]
    public class NewRequest : ICommand
    {
        public Request Request { get; set; }

        public NewRequest(object data, string commandName, string username)
        {
            Request = Request.CreateNewRequest(Serialize<object>(data), commandName, username);
        }

        public byte[] Serialize<TMessage>(TMessage message)
        {
            var s = new BinaryFormatter();
            var str = new MemoryStream();
            s.Serialize(str, message);
            str.Position = 0;
            var b = new byte[str.Length];
            str.Read(b, 0, (int)str.Length);

            return b;
        }
    }
}
