using System.Linq;
using AsbaBank.ApplicationService.Commands;
using AsbaBank.Core;
using AsbaBank.Core.Commands;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure.Logging;

namespace AsbaBank.ApplicationService
{
    public class ClientService : 
        IHandleCommand<RegisterClient>,
        IHandleCommand<UpdateClientAddress>
    {
        private readonly IUnitOfWork unitOfWork;
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(ClientService));

        public ClientService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Execute(RegisterClient command)
        {
            IRepository<Client> clientRepository = unitOfWork.GetRepository<Client>();

            try
            {
                var client = new Client(command.ClientName, command.ClientSurname, command.PhoneNumber);
                clientRepository.Add(client);
                unitOfWork.Commit();

                Logger.Verbose("Registered client {0} {1} with Id {2}", client.Name, client.Surname, client.Id);
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

                Logger.Verbose("Updated client address.");
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}