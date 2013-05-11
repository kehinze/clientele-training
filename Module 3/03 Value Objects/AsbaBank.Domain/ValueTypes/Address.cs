using System.Runtime.Serialization;

using AsbaBank.Core;

namespace AsbaBank.Domain.ValueTypes
{
    [DataContract]
    public struct Address
    {
        [DataMember]
        public string StreetNumber { get; private set; }
        [DataMember]
        public string Street { get; private set; }
        [DataMember]
        public string PostalCode { get; private set; }
        [DataMember]
        public string City { get; private set; }

        public static Address Empty
        {
            get
            {
                return new Address
                {
                    City = "Not yet captured",
                    PostalCode = "Not yet captured",
                    Street = "Not yet captured",
                    StreetNumber = "Not yet captured",
                };
            }
        }

        public Address(string streetNumber, string street, string postalCode, string city)
            : this()
        {
            Mandate.ParameterNotNullOrEmpty(streetNumber, "streetNumber");
            Mandate.ParameterNotNullOrEmpty(street, "street");
            Mandate.ParameterNotNullOrEmpty(postalCode, "postalCode");
            Mandate.ParameterNotNullOrEmpty(city, "city");

            StreetNumber = streetNumber;
            Street = street;
            PostalCode = postalCode;
            City = city;
        }
    }
}