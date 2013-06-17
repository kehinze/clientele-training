using System.ServiceModel;
using AsbaBank.ApplicationService.Dtos;

namespace AsbaBank.ApplicationService.Wcf
{
    [ServiceContract(Name = "ClientService", Namespace = "AsbaBank.Clients")]
    public class ClientServiceProxy : IClientService
    {
        private readonly IClientService clientService = Environment.GetClientService();

        [OperationContract]
        public void RegisterClient(string clientName, string clientSurname, string phoneNumber)
        {
            clientService.RegisterClient(clientName, clientSurname, phoneNumber);
        }

        [OperationContract]
        public void UpdateClientAddress(int clientId, string streetNumber, string street, string city, string postalCode)
        {
            clientService.UpdateClientAddress(clientId, streetNumber, street, city, postalCode);
        }

        [OperationContract]
        public ClientDto[] FetchAllClients()
        {
            return clientService.FetchAllClients();
        }
    }
}
