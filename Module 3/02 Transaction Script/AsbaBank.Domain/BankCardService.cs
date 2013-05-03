using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;

namespace AsbaBank.Domain
{
    public class BankCardService
    {
        private readonly IUnitOfWork unitOfWork;

        public BankCardService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<BankCard> GetAll()
        {
            return unitOfWork.GetRepository<BankCard>();
        }

        public BankCard Get(int bankCardId)
        {
            var bankCardRepository = unitOfWork.GetRepository<BankCard>();
            return bankCardRepository.Get(bankCardId);
        }

        public BankCard NewBankCard(int accountId)
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

        public BankCard DisableBankCard(int bankCardId)
        {
            try
            {
                var bankCardRepository = unitOfWork.GetRepository<BankCard>();
                var bankCard = bankCardRepository.Get(bankCardId);
                bankCard.Disabled = true;
                return bankCard;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}