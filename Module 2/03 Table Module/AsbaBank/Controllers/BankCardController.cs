using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AsbaBank.Domain;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;
using AsbaBank.Presentation.Mvc.ViewModelBuilders;
using AsbaBank.Presentation.Mvc.ViewModels;

namespace AsbaBank.Presentation.Mvc.Controllers
{
    public class BankCardController : Controller
    {
        private readonly BankCardModule bankCardModule;
        private readonly AccountModule accountModule;
        private readonly IUnitOfWork unitOfWork;

        public BankCardController()
        {
            unitOfWork = new InMemoryUnitOfWork(MvcApplication.DataStore);
            var accountRepository = unitOfWork.GetRepository<Account>();
            var clientModule = new ClientModule(unitOfWork.GetRepository<Client>());
            var transactionModule = new TransactionModule(unitOfWork.GetRepository<Transaction>());

            bankCardModule = new BankCardModule(unitOfWork.GetRepository<BankCard>());
            accountModule = new AccountModule(accountRepository, bankCardModule, transactionModule, clientModule);
        }

        public ActionResult Index()
        {
            IEnumerable<BankCardViewModel> viewModel = BankCardViewModelBuilder.Build(bankCardModule, accountModule);
            return View(viewModel);
        }

        public ActionResult Disable(int id = 0)
        {
            try
            {
                bankCardModule.Disable(id);
                unitOfWork.Commit();
                return RedirectToAction("Index");
            }
            catch 
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}