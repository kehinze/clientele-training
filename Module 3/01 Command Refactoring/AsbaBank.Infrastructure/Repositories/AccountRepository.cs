using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure.Interfaces;

namespace AsbaBank.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IRepository<Account> repository; 

        public AccountRepository(IUnitOfWork unitOfWork)
        {
            repository = unitOfWork.GetRepository<Account>();
        }

        public Account GetAccount(string accountNumber)
        {
            return repository.Single(c => c.AccountNumber == accountNumber);
        }
    }
}
