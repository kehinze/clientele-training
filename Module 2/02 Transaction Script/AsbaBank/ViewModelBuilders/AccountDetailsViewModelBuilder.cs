using System;
using System.Collections.Generic;
using AsbaBank.Domain;
using AsbaBank.Domain.Models;
using AsbaBank.Presentation.Mvc.ViewModels;

namespace AsbaBank.Presentation.Mvc.ViewModelBuilders
{
    public class AccountDetailsViewModelBuilder
    {
        public static AccountDetailsViewModel Build(AccountService accountService, int accountId)
        {
            var account = accountService.Get(accountId);

            return account == null 
                ? BuildInvalidAccountViewModel() 
                : BuildViewModel(accountService, account);
        }

        private static AccountDetailsViewModel BuildViewModel(AccountService accountService, Account account)
        {
            decimal balance = accountService.GetAccountBalance(account.Id);
            IEnumerable<Transaction> ledger = accountService.GetLedger(account.Id);
            Client client = accountService.GetAccountHolder(account.Id);

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