using System.Linq;
using AsbaBank.ApplicationService.Commands;
using AsbaBank.Core;
using AsbaBank.Core.Commands;
using AsbaBank.Domain.Models;

namespace AsbaBank.ApplicationService
{
    public class ClientService : 
        IHandleCommand<RegisterClient>,
        IHandleCommand<UpdateClientAddress>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog logger;

        public ClientService(IUnitOfWork unitOfWork, ILog logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public void Execute(RegisterClient command)
        {
            IRepository<Client> clientRepository = unitOfWork.GetRepository<Client>();

            try
            {
                var client = new Client(command.ClientName, command.ClientSurname, command.PhoneNumber);
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

        public void Execute(UpdateClientAddress command)
        {
            IRepository<Client> clientRepository = unitOfWork.GetRepository<Client>();
            Client client = clientRepository.Get(command.ClientId);

            try
            {
                client.UpdateAddress(command.StreetNumber, command.Street, command.City, command.PostalCode);
                unitOfWork.Commit();

                logger.Verbose("Updated client address.");
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}