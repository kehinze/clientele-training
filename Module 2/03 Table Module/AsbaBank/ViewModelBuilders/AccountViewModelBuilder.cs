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
        public static IEnumerable<AccountViewModel> Build(AccountModule accountModule)
        {
            var accounts = accountModule.GetAll();

            return accounts.Select(account => BuildViewModel(accountModule, account));
        }

        private static AccountViewModel BuildViewModel(AccountModule accountModule, Account account)
        {
            decimal balance = accountModule.GetAccountBalance(account.Id);
            Client client = accountModule.GetAccountHolder(account.Id);

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