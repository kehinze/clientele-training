using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;

namespace AsbaBank.Domain
{
    public class BankCardModule
    {
        private readonly IRepository<BankCard> bankCardRepository;

        public BankCardModule(IRepository<BankCard> bankCardRepository)
        {
            this.bankCardRepository = bankCardRepository;
        }

        public IEnumerable<BankCard> GetAll()
        {
            return bankCardRepository;
        }

        public BankCard Get(int bankCardId)
        {
            return bankCardRepository.Get(bankCardId);
        }

        public void DisableAccountBankCards(int accountId)
        {
            var bankCards = bankCardRepository.Where(card => card.AccountId == accountId);

            foreach (var bankCard in bankCards)
            {
                bankCard.Disabled = true;
            }
        }

        public void Disable(int bankCardId)
        {
            var bankCard = bankCardRepository.Get(bankCardId);
            bankCard.Disabled = true;
        }

        public void IssueBankCard(int accountId)
        {
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
        }
    }
}