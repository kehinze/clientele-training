using System;
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
                Validator.ValidateObject(client, new ValidationContext(client));
                var clientRepository = unitOfWork.GetRepository<Client>();
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

        public Account OpenAccount(int clientId)
        {
            try
            {
                var clientRepository = unitOfWork.GetRepository<Client>();

                if (clientRepository.All(client => client.Id != clientId))
                {
                    throw new ValidationException("The provided client id does not exist.");
                }

                var newAccount = new Account
                {
                    AccountNumber = GenerateAccountNumber(),
                    ClientId = clientId,
                    Closed = false
                };

                Validator.ValidateObject(newAccount, new ValidationContext(newAccount));
                var accountRepository = unitOfWork.GetRepository<Account>();
                accountRepository.Add(newAccount);
                unitOfWork.Commit();
                return newAccount;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private static string GenerateAccountNumber()
        {
            var ticks = DateTime.Now.Ticks.ToString();
            return ticks.Substring(ticks.Length - 10);
        }
    }
}