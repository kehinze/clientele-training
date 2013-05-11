using System;
using System.Runtime.Serialization;

using AsbaBank.Core;

namespace AsbaBank.Domain.ValueTypes
{
    [DataContract]
    public struct PersonName : IEquatable<PersonName>
    {
        [DataMember]
        public string FirstName { get; private set; }
        [DataMember]
        public string Surname { get; private set; }

        public PersonName(string firstName, string surname)
            : this()
        {
            Mandate.ParameterNotNullOrEmpty(firstName, "firstName");
            Mandate.ParameterNotNullOrEmpty(surname, "surname");

            FirstName = firstName;
            Surname = surname;
        }

        public override int GetHashCode()
        {
            return FirstName.GetHashCode() ^ Surname.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is PersonName && Equals((PersonName)obj);
        }

        public bool Equals(PersonName other)
        {
            return other.FirstName == FirstName
                && other.Surname == Surname;
        }

        public static bool operator ==(PersonName left, PersonName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PersonName left, PersonName right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return String.Format("{0} {1}", FirstName, Surname);
        }
    }
}
