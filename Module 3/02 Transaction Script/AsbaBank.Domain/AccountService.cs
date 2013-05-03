using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
                var clientRepository = unitOfWork.GetRepository<Client>();

                if(clientRepository.All(client => client.Id != account.ClientId))
                {
                    throw new ValidationException("The provided client id does not exist.");
                }

                var accountRepository = unitOfWork.GetRepository<Account>();

                if (accountRepository.Any(a => a.AccountNumber == account.AccountNumber))
                {
                    throw new ValidationException("The account already exists.");
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

        public BankCard IssueBankCard(int accountId)
        {
            try
            {
                var accountRepository = unitOfWork.GetRepository<Account>();
                var account = accountRepository.Get(accountId);

                if (account.Closed)
                {
                    throw new ValidationException("The account is closed");
                }

                var bankCardRepository = unitOfWork.GetRepository<BankCard>();

                if (bankCardRepository.Any(card => card.AccountId == accountId && !card.Disabled))
                {
                    throw new ValidationException("An account may only have one active bank card at a time.");
                }

                var newBankCard = new BankCard
                {
                    AccountId = accountId,
                    Disabled = false,
                };

                Validator.ValidateObject(newBankCard, new ValidationContext(newBankCard));

                bankCardRepository.Add(newBankCard);
                unitOfWork.Commit();
                return newBankCard;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public Account Credit(int accountId, decimal amount)
        {
            try
            {
                var accountRepository = unitOfWork.GetRepository<Account>();
                var account = accountRepository.Get(accountId);

                if (account.Closed)
                {
                    throw new ValidationException("The account is closed");
                }

                account.Balance += amount;
                unitOfWork.Commit();
                return account;
            }
            catch 
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public Account Debit(int accountId, decimal amount)
        {
            try
            {
                var accountRepository = unitOfWork.GetRepository<Account>();
                var account = accountRepository.Get(accountId);

                if (account.Closed)
                {
                    throw new ValidationException("The account is closed");
                }

                if (account.Balance < amount)
                {
                    throw new ValidationException("Insufficient balance.");
                }

                account.Balance -= amount;
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
