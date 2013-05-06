using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AsbaBank.Domain;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;
using AsbaBank.Presentation.Mvc.Forms;
using AsbaBank.Presentation.Mvc.ViewModelBuilders;
using AsbaBank.Presentation.Mvc.ViewModels;

namespace AsbaBank.Presentation.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountModule accountModule;
        private readonly IUnitOfWork unitOfWork;

        public AccountController()
        {
            unitOfWork = MvcApplication.UnitOfWork;
            var accountRepository = unitOfWork.GetRepository<Account>();
            var bankCardModule = new BankCardModule(unitOfWork.GetRepository<BankCard>());
            var clientModule = new ClientModule(unitOfWork.GetRepository<Client>());
            var transactionModule = new TransactionModule(unitOfWork.GetRepository<Transaction>());

            accountModule = new AccountModule(accountRepository, bankCardModule, transactionModule, clientModule);
        }

        public ActionResult Index()
        {
            IEnumerable<AccountViewModel> accountViewModels = AccountViewModelBuilder.Build(accountModule);
          
            return View(accountViewModels);
        }

        public ActionResult Details(int id = 0)
        {
            AccountDetailsViewModel viewModel = AccountDetailsViewModelBuilder.Build(accountModule, id);

            return View(viewModel);
        }

        public ActionResult Close(int id = 0)
        {
            try
            {
                accountModule.Close(id);
                unitOfWork.Commit();
                return RedirectToAction("Index");
            }
            catch 
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public ActionResult Debit(int id = 0)
        {
            var account = accountModule.Get(id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return View(new AccountDebitForm
            {
                AccountId = account.Id,
                DebitAmount = 0
            });
        }

        [HttpPost]
        public ActionResult Debit(AccountDebitForm form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    accountModule.Debit(form.AccountId, form.DebitAmount);
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
                catch 
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return View("Debit", form);
        }

        public ActionResult Credit(int id = 0)
        {
            var account = accountModule.Get(id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return View(new AccountCreditForm
            {
                AccountId = account.Id,
                CreditAmount = 0
            });
        }

        [HttpPost]
        public ActionResult Credit(AccountCreditForm form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    accountModule.Credit(form.AccountId, form.CreditAmount);
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
            
            return View("Credit", form);
        }

        public ActionResult OpenAccount(int clientId)
        {
            try
            {
                accountModule.OpenAccount(clientId);
                unitOfWork.Commit();
                return RedirectToAction("Index");
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public ActionResult IssueBankCard(int id = 0)
        {
            try
            {
                accountModule.IssueBankCard(id);
                unitOfWork.Commit();
                return RedirectToAction("Index", "BankCard");
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }        
    }
}