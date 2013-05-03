using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;

namespace AsbaBank.Domain
{
    public class AccountService
    {
        private readonly IUnitOfWork unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Account> GetAll()
        {
            return unitOfWork.GetRepository<Account>();
        }   

        public Account Get(int accountId)
        {
            var accountRepository = unitOfWork.GetRepository<Account>();
            return accountRepository.Get(accountId);
        }

        public Account Create(Account account)
        {
            try
            {
                var accountRepository = unitOfWork.GetRepository<Account>();

                if (accountRepository.Any(a => a.AccountNumber == account.AccountNumber))
                {
                    throw new ValidationException("The account already exists.");
                }

                var clientRepository = unitOfWork.GetRepository<Client>();

                if (clientRepository.Any(client => client.Id == account.ClientId && !client.Active))
                {
                    throw new ValidationException("Unable to open an account for a client that is not active.");
                }

                accountRepository.Add(account);
                unitOfWork.Commit();
                return account;
            }
            catch 
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public Account Update(Account account)
        {
            try
            {
                var accountRepository = unitOfWork.GetRepository<Account>();
                Validator.ValidateObject(account, new ValidationContext(account));
                accountRepository.Update(account.Id, account);
                unitOfWork.Commit();
                return account;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public Account Close(int accountId)
        {
            try
            {
                var accountRepository = unitOfWork.GetRepository<Account>();
                var account = accountRepository.Get(accountId);

                if (account.Closed)
                {
                    throw new ValidationException("The account is already closed");
                }

                account.Closed = true;

                var bankCardRepository = unitOfWork.GetRepository<BankCard>();

                var bankCard = bankCardRepository.SingleOrDefault(card => card.AccountId == account.Id && card.Disabled == false);

                if (bankCard != null)
                {
                    bankCard.Disabled = true;
                }

                Validator.ValidateObject(account, new ValidationContext(account));
                unitOfWork.Commit();
                return account;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}
