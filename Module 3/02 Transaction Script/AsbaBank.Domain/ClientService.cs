using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;

namespace AsbaBank.Domain
{
    public class ClientService
    {
        private readonly IUnitOfWork unitOfWork;

        public ClientService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Client> GetAll()
        {
            return unitOfWork.GetRepository<Client>();
        }

        public Client Get(int clientId)
        {
            var clientRepository = unitOfWork.GetRepository<Client>();
            return clientRepository.Get(clientId);
        }

        public Client Create(Client client)
        {
            try
            {
                var clientRepository = unitOfWork.GetRepository<Client>();
                Validator.ValidateObject(client, new ValidationContext(client));
                clientRepository.Add(client);
                unitOfWork.Commit();
                return client;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public Client Update(Client client)
        {
            try
            {
                var clientRepository = unitOfWork.GetRepository<Client>();
                Validator.ValidateObject(client, new ValidationContext(client));
                clientRepository.Update(client.Id, client);
                unitOfWork.Commit();
                return client;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}