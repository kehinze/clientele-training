using System.Runtime.Serialization;

namespace AsbaBank.Core.Messaging
{
    [DataContract]
    public class Envelope
    {
        [DataMember]
        public byte[] Message { get; set; }
    }
}