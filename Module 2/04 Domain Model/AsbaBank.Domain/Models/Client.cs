using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AsbaBank.Domain.Models
{
    [DataContract]
    public class Client
    {
        [Key, DataMember]
        public int Id { get; protected set; }
        [DataMember]
        public string ClientName { get; protected set; }
        [DataMember]
        public string PhoneNumber { get; protected set; }
        [DataMember]
        public Address Address { get; protected set; }

        protected Client()
        {
            //here for the deserializer
        }

        public Client(string clientName, string phoneNumber)
        {
            if (String.IsNullOrWhiteSpace(clientName))
            {
                throw new ArgumentException("Please provide a valid client name.");
            }

            if (String.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length != 10 || !phoneNumber.IsDigitsOnly())
            {
                throw new ArgumentException("Please provide a valid telephone number.");
            }

            ClientName = clientName;
            PhoneNumber = phoneNumber;
            Address = Address.NullAddress();
        }
    }
}