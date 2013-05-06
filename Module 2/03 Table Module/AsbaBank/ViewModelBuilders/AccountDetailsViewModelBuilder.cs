using System;
using System.Collections.Generic;
using AsbaBank.Domain;
using AsbaBank.Domain.Models;
using AsbaBank.Presentation.Mvc.ViewModels;

namespace AsbaBank.Presentation.Mvc.ViewModelBuilders
{
    public class AccountDetailsViewModelBuilder
    {
        public static AccountDetailsViewModel Build(AccountModule accountModule, int accountId)
        {
            var account = accountModule.Get(accountId);

            return account == null 
                ? BuildInvalidAccountViewModel()
                : BuildViewModel(accountModule, account);
        }

        private static AccountDetailsViewModel BuildViewModel(AccountModule accountModule, Account account)
        {
            decimal balance = accountModule.GetAccountBalance(account.Id);
            IEnumerable<Transaction> ledger = accountModule.GetLedger(account.Id);
            Client client = accountModule.GetAccountHolder(account.Id);

            return new AccountDetailsViewModel
            {
                AccountNumber = account.AccountNumber,
                Balance = balance.ToString("C"),
                Id = account.Id,
                ClientName = client.ClientName,
                Status = account.Closed ? "Closed" : "Open",
                Ledger = ledger
            };
        }

        private static AccountDetailsViewModel BuildInvalidAccountViewModel()
        {
            return new AccountDetailsViewModel
            {
                AccountNumber = string.Empty,
                Balance = string.Empty,
                Id = 0,
                ClientName = String.Empty,
                Status = "Invalid Account",
                Ledger = new Transaction[0]
            };
        }
    }
}