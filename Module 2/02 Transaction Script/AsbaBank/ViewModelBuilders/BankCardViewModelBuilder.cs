using System.Collections.Generic;
using System.Linq;
using AsbaBank.Domain;
using AsbaBank.Domain.Models;
using AsbaBank.Presentation.Mvc.ViewModels;

namespace AsbaBank.Presentation.Mvc.ViewModelBuilders
{
    public class BankCardViewModelBuilder
    {
        public static IEnumerable<BankCardViewModel> Build(BankCardService bankCardService)
        {
            IEnumerable<BankCard> bankCards = bankCardService.GetAll();

            return bankCards.Select(bankCard => BuildViewModel(bankCardService, bankCard));
        }

        private static BankCardViewModel BuildViewModel(BankCardService bankCardService, BankCard bankCard)
        {
            var bankAccount = bankCardService.GetAccount(bankCard.Id);

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