using System.Linq;
using AsbaBank.ApplicationService.Dtos;
using AsbaBank.Core;
using AsbaBank.Domain.Models;

namespace AsbaBank.ApplicationService
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog logger;

        public ClientService(IUnitOfWork unitOfWork, ILog logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public void RegisterClient(string clientName, string clientSurname, string phoneNumber)
        {
            Mandate.ParameterNotNullOrEmpty(clientName, "clientName");
            Mandate.ParameterNotNullOrEmpty(clientSurname, "clientSurname");
            Mandate.ParameterNotNullOrEmpty(phoneNumber, "phoneNumber");
            Mandate.ParameterCondition(phoneNumber.Length == 10 && phoneNumber.IsDigitsOnly(), "phoneNumber", "Please provide a valid phone number that is 10 digits long.");

            IRepository<Client> clientRepository = unitOfWork.GetRepository<Client>();

            try
            {
                var client = new Client(clientName, clientSurname, phoneNumber);
                clientRepository.Add(client);
                unitOfWork.Commit();

                logger.Verbose("Registered client {0} {1} with Id {2}", client.Name, client.Surname, client.Id);
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public void UpdateClientAddress(int clientId, string streetNumber, string street, string city, string postalCode)
        {
            Mandate.ParameterNotNullOrEmpty(streetNumber, "streetNumber");
            Mandate.ParameterNotNullOrEmpty(street, "street");
            Mandate.ParameterNotNullOrEmpty(postalCode, "postalCode");
            Mandate.ParameterNotNullOrEmpty(city, "city");

            IRepository<Client> clientRepository = unitOfWork.GetRepository<Client>();
            Client client = clientRepository.Get(clientId);

            try
            {
                client.UpdateAddress(streetNumber, street, city, postalCode);
                unitOfWork.Commit();

                logger.Verbose("Updated client address.");
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public ClientDto[] FetchAllClients()
        {
            IRepository<Client> clientRepository = unitOfWork.GetRepository<Client>();
            var mapper = new ClientDtoMapper();

            return clientRepository.Select(mapper.Expression).ToArray();
        }
    }
}