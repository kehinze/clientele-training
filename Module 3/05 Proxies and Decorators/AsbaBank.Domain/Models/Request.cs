using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AsbaBank.Domain.Models
{
    public class Request
    {
        [Key]
        public virtual Guid Id { get; protected set; }
        public virtual byte[] Data { get; protected set; }
        public virtual DateTime Created { get; protected set; }
        public string CommandName { get; protected set; }
        public string Username { get; set; }

        public static Request CreateNewRequest(byte[] data, string commandName, string username)
        {
            return new Request
                {
                    Id = Guid.NewGuid(),
                    Data = data,
                    Created = DateTime.Now,
                    CommandName = commandName,
                    Username = username
                };
        }

    }
}
