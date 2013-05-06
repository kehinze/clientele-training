using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;

namespace AsbaBank.Domain
{
    public class TransactionModule
    {
        private readonly IRepository<Transaction> transactionRepository;

        public TransactionModule(IRepository<Transaction> transactionRepository)
        {
            this.transactionRepository = transactionRepository;
        }

        internal void DebitAccount(int accountId, decimal amount)
        {
            decimal balance = GetAccountBalance(accountId);

            if (balance < amount)
            {
                throw new ValidationException("Insufficient balance.");
            }

            RegisterTransaction(accountId, -amount);
        }

        internal void CreditAccount(int accountId, decimal amount)
        {
            RegisterTransaction(accountId, amount);
        }

        private void RegisterTransaction(int accountId, decimal amount)
        {
            transactionRepository.Add(new Transaction
            {
                TransactionAmount = amount,
                AccountId = accountId,
                TransactionDate = DateTime.Now
            });
        }

        internal decimal GetAccountBalance(int accountId)
        {
            return transactionRepository
                .Where(ledger => ledger.AccountId == accountId)
                .Sum(ledger => ledger.TransactionAmount);
        }

        internal IEnumerable<Transaction> GetLedger(int accountId)
        {
            return transactionRepository.Where(transaction => transaction.AccountId == accountId);
        }
    }
}