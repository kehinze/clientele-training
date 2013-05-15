using System;
using System.Runtime.Serialization;

namespace AsbaBank.Domain.Models
{
    [DataContract]
    public class Address
    {
        [DataMember]
        public string StreetNumber { get; protected set; }
        [DataMember]
        public string Street { get; protected set; }
        [DataMember]
        public string PostalCode { get; protected set; }
        [DataMember]
        public string City { get; protected set; }

        protected Address()
        {
            //here for the deserializer
        }

        public Address(string streetNumber, string street, string city, string postalCode)
        {
            ValidateInput("street number", streetNumber);
            ValidateInput("street", street);
            ValidateInput("postal code", postalCode);
            ValidateInput("city", city);
        }

        public static Address NullAddress()
        {
            return new Address
            {
                City = "Not yet captured",
                PostalCode = "Not yet captured",
                Street = "Not yet captured",
                StreetNumber = "Not yet captured",
            };
        }

        private void ValidateInput(string parameterName, string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(String.Format("Please provide a valid {0}.", parameterName));
            }
        }
    }
}