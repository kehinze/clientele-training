using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using AsbaBank.Core;
using AsbaBank.Domain.ValueTypes;

namespace AsbaBank.Domain.Models
{
    [DataContract]
    public class Client
    {
        [Key, DataMember]
        public int Id { get; protected set; }
        [DataMember]
        public PersonName ClientName { get; protected set; }
        [DataMember]
        public TelephoneNumber PhoneNumber { get; protected set; }
        [DataMember]
        public Address Address { get; protected set; }

        protected Client()
        {
            //here for the deserializer
        }

        public Client(PersonName clientName, TelephoneNumber phoneNumber)
        {
            Mandate.ParameterNotDefaut(clientName, "clientName");
            Mandate.ParameterNotDefaut(phoneNumber, "phoneNumber");

            ClientName = clientName;
            PhoneNumber = phoneNumber;
            Address = Address.Empty;
        }
        
        public override string ToString()
        {
            return String.Format("[{0}] {1} {2}", Id, ClientName, PhoneNumber);
        }
    }
}