using System.Collections.Generic;
using System.Linq;
using AsbaBank.Domain;
using AsbaBank.Domain.Models;
using AsbaBank.Presentation.Mvc.ViewModels;

namespace AsbaBank.Presentation.Mvc.ViewModelBuilders
{
    public class BankCardViewModelBuilder
    {
        public static IEnumerable<BankCardViewModel> Build(BankCardModule bankCardModule, AccountModule accountModule)
        {
            IEnumerable<BankCard> bankCards = bankCardModule.GetAll();

            return bankCards.Select(bankCard => BuildViewModel(bankCardModule, accountModule, bankCard));
        }

        private static BankCardViewModel BuildViewModel(BankCardModule bankCardModule, AccountModule accountModule, BankCard bankCard)
        {
            var bankAccount = accountModule.Get(bankCard.AccountId);

            return new BankCardViewModel
            {
                Id = bankCard.Id,
                AccountNumber = bankAccount.AccountNumber,
                Status = bankCard.Disabled ? "Disabled" : "Active",
                Disabled = bankCard.Disabled
            };
        }
    }
}