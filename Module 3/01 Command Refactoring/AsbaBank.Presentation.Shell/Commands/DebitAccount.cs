using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsbaBank.Presentation.Shell.Startup;
using Ninject;
using AsbaBank.Infrastructure.Interfaces;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure.Repositories;

namespace AsbaBank.Presentation.Shell.Commands
{
    public class DebitAccount : ICommand, IShellCommand 
    {
        public decimal Amount { get; private set; }
        public string AccountNumber { get; private set; }

        public DebitAccount(string accountNumber, decimal amount)
        {
            Amount = amount;
            AccountNumber = accountNumber;
        }

        public string Usage { get { return string.Format("{0} <AcccountId> <Amount>", Key); } }
        public string Key { get { return "DebitAccount"; } }

        public ICommand Build(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException(String.Format("Incorrect number of parameters. Usage is: {0}", Usage));
            }
            return new DebitAccount(args[0], args[1]);
        }

        public void Execute()
        {
            using (var kernel = new StandardKernel(new AbsaBankModules()))
            {
                var unitOfWork = kernel.Get<IUnitOfWork>();
                var accountRepository = kernel.Get<IAccountRepository>();

                try
                {
                    var account = accountRepository.GetAccount(AccountNumber);

                    account.Debit(Amount);

                    unitOfWork.Commit();
                }
                catch 
                {
                    unitOfWork.Rollback();

                    throw;
                }
               
            }
        }

        
    }
}
