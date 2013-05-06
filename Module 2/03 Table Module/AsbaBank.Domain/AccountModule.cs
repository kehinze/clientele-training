using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;

namespace AsbaBank.Domain
{
    public class AccountModule
    {
        private readonly IRepository<Account> accountRepository;
        private readonly BankCardModule bankCardModule;
        private readonly TransactionModule transactionModule;
        private readonly ClientModule clientModule;

        public AccountModule(IRepository<Account> accountRepository, BankCardModule bankCardModule, TransactionModule transactionModule, ClientModule clientModule)
        {
            this.accountRepository = accountRepository;
            this.bankCardModule = bankCardModule;
            this.transactionModule = transactionModule;
            this.clientModule = clientModule;
        }

        public IEnumerable<Account> GetAll()
        {
            return accountRepository;
        }

        public Account Get(int clientId)
        {
            return accountRepository.Get(clientId);
        }

        public Account OpenAccount(int clientId)
        {
            if (clientModule.Get(clientId) == null)
            {
                throw new ValidationException("The client could not be found.");
            }

            var newAccount = new Account
            {
                AccountNumber = GenerateAccountNumber(),
                ClientId = clientId,
                Closed = false
            };

            Validator.ValidateObject(newAccount, new ValidationContext(newAccount));
            accountRepository.Add(newAccount);
            return newAccount;
        }

        private static string GenerateAccountNumber()
        {
            var ticks = DateTime.Now.Ticks.ToString();
            return ticks.Substring(ticks.Length - 10);
        }

        public void Debit(int accountId, decimal amount)
        {
            var account = accountRepository.Get(accountId);

            if (account.Closed)
            {
                throw new ValidationException("The account is closed");
            }

            transactionModule.DebitAccount(accountId, amount);
        }

        public void Credit(int accountId, decimal amount)
        {
            var account = accountRepository.Get(accountId);

            if (account.Closed)
            {
                throw new ValidationException("The account is closed");
            }

            transactionModule.CreditAccount(accountId, amount);
        }

        public void Close(int accountId)
        {
            var account = accountRepository.Get(accountId);

            if (account.Closed)
            {
                throw new ValidationException("The account is already closed");
            }

            account.Closed = true;
            bankCardModule.DisableAccountBankCards(accountId);
        }

        public void IssueBankCard(int accountId)
        {
            var account = accountRepository.Get(accountId);

            if (account.Closed)
            {
                throw new ValidationException("The account is closed");
            }

            bankCardModule.IssueBankCard(accountId);
        }

        public Client GetAccountHolder(int accountId)
        {
            var account = accountRepository.Get(accountId);
            return clientModule.Get(account.ClientId);
        }

        public decimal GetAccountBalance(int accountId)
        {
            return transactionModule.GetAccountBalance(accountId);
        }

        public IEnumerable<Transaction> GetLedger(int accountId)
        {
            return transactionModule.GetLedger(accountId);
        }
    }
}