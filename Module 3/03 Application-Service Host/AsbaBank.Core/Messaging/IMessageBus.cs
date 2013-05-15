using System.ServiceModel;

namespace AsbaBank.Core.Messaging
{
    [ServiceContract]
    public interface IMessageBus
    {
        [OperationContract]
        void Send(Envelope value);
    }
}
