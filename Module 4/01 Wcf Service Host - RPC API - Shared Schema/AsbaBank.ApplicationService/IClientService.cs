using AsbaBank.ApplicationService.Dtos;

namespace AsbaBank.ApplicationService
{
    public interface IClientService
    {
        void RegisterClient(string clientName, string clientSurname, string phoneNumber);
        void UpdateClientAddress(int clientId, string streetNumber, string street, string city, string postalCode);
        ClientDto[] FetchAllClients();
    }
}