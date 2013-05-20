using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AsbaBank.Core.Commands;

namespace AsbaBank.ApplicationService.Commands
{
    [DataContract]
    public class UpdateClientNameAndSurname :  ICommand
    {
        [DataMember]
        public int ClientId { get; protected set; }
        [DataMember]
        public string Name { get; protected set; }
        [DataMember]
        public string Surname { get; protected set; }

        public UpdateClientNameAndSurname(int clientId, string name, string surname)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
            {
                throw new ArgumentException("Please provide a valid name and surname.");
            }
            ClientId = clientId;
            Name = name;
            Surname = surname;
        }
    }
}
