using System;
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

            var client = new Client(command.ClientName, command.ClientSurname, command.PhoneNumber);
            clientRepository.Add(client);
            
            //this code is here to simulate a transient error happening on the network
            if (DateTime.Now.Second % 2 == 0)
            {
                throw new Exception("Some random error has happened. This would normally mean our user must retry their action.");
            }
            
            unitOfWork.Commit();

            Logger.Info("Registered client {0} {1} with Id {2}", client.Name, client.Surname, client.Id);
        }

        public void Execute(UpdateClientAddress command)
        {
            IRepository<Client> clientRepository = unitOfWork.GetRepository<Client>();
            Client client = clientRepository.Get(command.ClientId);

            client.UpdateAddress(command.StreetNumber, command.Street, command.City, command.PostalCode);
            unitOfWork.Commit();

            Logger.Info("Updated client address.");
        }
    }
}