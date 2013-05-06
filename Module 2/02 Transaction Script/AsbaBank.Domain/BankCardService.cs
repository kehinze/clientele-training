using System.Collections.Generic;
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

        public Account GetAccount(int cardId)
        {
            var bankCardRepository = unitOfWork.GetRepository<BankCard>();
            var bankCard = bankCardRepository.Get(cardId);

            var accountRepository = unitOfWork.GetRepository<Account>();
            return accountRepository.Get(bankCard.AccountId);
        }
    }
}