using System;
using System.Runtime.Serialization;

namespace AsbaBank.Domain.Models
{
    [DataContract]
    public class Transaction
    {
        [DataMember]
        public decimal Amount { get; protected set; }
        [DataMember]
        public DateTime Date { get; protected set; }

        protected Transaction()
        {
            //here for the deserializer
        }

        protected Transaction(decimal amount, DateTime date)
        {
            Amount = amount;
            Date = date;
        }

        internal static Transaction CreditTransaction(decimal amount)
        {
            return new Transaction(Math.Abs(amount), DateTime.Now);
        }

        internal static Transaction DebitTransaction(decimal amount)
        {
            return new Transaction(-Math.Abs(amount), DateTime.Now);
        }
    }
}