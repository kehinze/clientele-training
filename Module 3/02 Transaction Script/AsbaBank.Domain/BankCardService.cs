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

        public Account Withdraw(int cardId, decimal amount)
        {
            try
            {
                var bankCardRepository = unitOfWork.GetRepository<BankCard>();
                var bankCard = bankCardRepository.Get(cardId);

                if (bankCard.Disabled)
                {
                    throw new ValidationException("The bank card is disabled.");
                }

                var accountRepository = unitOfWork.GetRepository<Account>();
                var account = accountRepository.Get(bankCard.AccountId);

                if (account.Closed)
                {
                    throw new ValidationException("The account is closed.");
                }

                if (account.Balance < amount)
                {
                    throw new ValidationException("Insufficient balance.");
                }

                account.Balance -= amount;
                unitOfWork.Commit();
                return account;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public BankCard Disable(int bankCardId)
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