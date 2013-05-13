using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using AsbaBank.Core;

namespace AsbaBank.Domain.Models
{
    [DataContract]
    public class Client
    {
        [Key]
        [DataMember]
        public int Id { get; protected set; }

        [DataMember]
        public string Name { get; protected set; }

        [DataMember]
        public string Surname { get; protected set; }

        [DataMember]
        public string PhoneNumber { get; protected set; }

        [DataMember]
        public Address Address { get; protected set; }

        protected Client()
        {
            //here for the deserializer
        }

        public Client(string clientName, string clientSurname, string phoneNumber)
        {
            if (String.IsNullOrEmpty(clientName) || clientName.Length < 3)
            {
                throw new ArgumentException("Please provide a valid client name of at least three characters.");
            }

            if (String.IsNullOrEmpty(clientSurname) || clientSurname.Length < 3)
            {
                throw new ArgumentException("Please provide a valid client name of at least three characters.");
            }

            if (String.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length != 10 || !phoneNumber.IsDigitsOnly())
            {
                throw new ArgumentException("Please provide a valid telephone number.");
            }

            Name = clientName;
            Surname = clientSurname;
            PhoneNumber = phoneNumber;
            Address = Address.NullAddress();
        }

        public void UpdateAddress(string streetNumber, string street, string postalCode, string city)
        {
            Address = new Address(streetNumber, street, postalCode, city);
        }
    }
}