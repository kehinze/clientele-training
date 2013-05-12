using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsbaBank.Infrastructure.Interfaces;
using AsbaBank.Domain.Models;

namespace AsbaBank.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ILog log;
        private readonly IRepository<Client> clientRepository;
        
        public ClientRepository(IUnitOfWork unitOfWork)
        {
            this.log = log;
            clientRepository = unitOfWork.GetRepository<Client>();
        }

        public void RegisterClient(Client client)
        {
              clientRepository.Add(client);
        }


    }
}
