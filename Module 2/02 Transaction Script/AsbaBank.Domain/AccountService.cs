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

                var transactionRepository = unitOfWork.GetRepository<Transaction>();

                transactionRepository.Add(new Transaction
                {
                    TransactionAmount = amount,
                    AccountId = account.Id,
                    TransactionDate = DateTime.Now
                });

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

                decimal balance = GetAccountBalance(accountId);

                if (balance < amount)
                {
                    throw new ValidationException("Insufficient balance.");
                }

                var transactionRepository = unitOfWork.GetRepository<Transaction>();

                transactionRepository.Add(new Transaction
                {
                    TransactionAmount = -amount,
                    AccountId = account.Id,
                    TransactionDate = DateTime.Now
                });

                unitOfWork.Commit();
                return account;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public decimal GetAccountBalance(int accountId)
        {
            var transactionRepository = unitOfWork.GetRepository<Transaction>();

            return transactionRepository
                .Where(ledger => ledger.AccountId == accountId)
                .Sum(ledger => ledger.TransactionAmount);
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

        public IEnumerable<Transaction> GetLedger(int accountId)
        {
            var transactionRepository = unitOfWork.GetRepository<Transaction>();

            return transactionRepository.Where(transaction => transaction.AccountId == accountId);
        }

        public Client GetAccountHolder(int accountId)
        {
            var accountRepository = unitOfWork.GetRepository<Account>();
            var account = accountRepository.Get(accountId);

            var clientRepository = unitOfWork.GetRepository<Client>();
            return clientRepository.Get(account.ClientId);
        }

    }
}
