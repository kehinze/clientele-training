using System;
using System.Collections.Generic;
using System.Linq;
using AsbaBank.Domain;
using AsbaBank.Domain.Models;
using AsbaBank.Presentation.Mvc.ViewModels;

namespace AsbaBank.Presentation.Mvc.ViewModelBuilders
{
    public class AccountViewModelBuilder
    {
        public static IEnumerable<AccountViewModel> Build(AccountService accountService)
        {
            var accounts = accountService.GetAll();

            return accounts.Select(account => BuildViewModel(accountService, account));
        }

        private static AccountViewModel BuildViewModel(AccountService accountService, Account account)
        {
            decimal balance = accountService.GetAccountBalance(account.Id);
            IEnumerable<Transaction> ledger = accountService.GetLedger(account.Id);
            Client client = accountService.GetAccountHolder(account.Id);

            return new AccountViewModel
            {
                AccountNumber = account.AccountNumber,
                Balance = balance.ToString("C"),
                Id = account.Id,
                ClientName = client.ClientName,
                Status = account.Closed ? "Closed" : "Open",
                Closed = account.Closed
            };
        }
    }
}