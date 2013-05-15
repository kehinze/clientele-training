using System;
using System.Runtime.Serialization;

namespace AsbaBank.Domain.Models
{
    [DataContract]
    public class BankCard
    {
        [DataMember]
        public int Id { get; protected set; }
        [DataMember]
        public int AccountId { get; protected set; }
        [DataMember]
        public bool Disabled { get; protected set; }
        [DataMember]
        public DateTime Issued { get; protected set; }

        protected BankCard()
        {
            //here for the deserializer
        }

        internal BankCard(int id, int accountId)
        {
            Id = id;
            AccountId = accountId;
            Issued = DateTime.Now;
        }

        public void Disable()
        {
            Disabled = true;
        }
    }
}