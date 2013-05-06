using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;

namespace AsbaBank.Domain
{
    public class ClientModule
    {
        private readonly IRepository<Client> clientRepository;

        public ClientModule(IRepository<Client> clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public IEnumerable<Client> GetAll()
        {
            return clientRepository;
        }

        public Client Get(int clientId)
        {
            return clientRepository.Get(clientId);
        }

        public Client Create(Client client)
        {
            Validator.ValidateObject(client, new ValidationContext(client));
            clientRepository.Add(client);
            return client;
        }

        public void Update(Client client)
        {
            Validator.ValidateObject(client, new ValidationContext(client));
            clientRepository.Update(client.Id, client);
        }
    }
}