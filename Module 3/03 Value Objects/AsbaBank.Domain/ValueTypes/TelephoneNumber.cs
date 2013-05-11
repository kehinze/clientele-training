using System;
using System.Runtime.Serialization;

using AsbaBank.Core;

namespace AsbaBank.Domain.ValueTypes
{
    [DataContract]
    public struct TelephoneNumber : IEquatable<TelephoneNumber>
    {
        [DataMember] private long number;

        public TelephoneNumber(string number)
            : this()
        {
            Mandate.ParameterNotNullOrEmpty(number, "number");
            Mandate.ParameterCondition(number.Length >= 10 && number.IsDigitsOnly(), "number", "A telephone number must be 10 digits long.");

            this.number = Int64.Parse(number);
        }

        public override int GetHashCode()
        {
            return number.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is TelephoneNumber && Equals((TelephoneNumber)obj);
        }

        public bool Equals(TelephoneNumber other)
        {
            return other.number == number;
        }

        public static bool operator ==(TelephoneNumber left, TelephoneNumber right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TelephoneNumber left, TelephoneNumber right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return String.Format("{0:0##-###-####}", number);
        }
    }
}