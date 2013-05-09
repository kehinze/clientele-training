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
        private readonly AccountService accountService;

        public AccountController()
        {
            accountService = new AccountService(new InMemoryUnitOfWork(MvcApplication.DataStore));
        }

        public ActionResult Index()
        {
            IEnumerable<AccountViewModel> accountViewModels = AccountViewModelBuilder.Build(accountService);

            return View(accountViewModels);
        }

        public ActionResult Details(int id = 0)
        {
            AccountDetailsViewModel viewModel = AccountDetailsViewModelBuilder.Build(accountService, id);

            return View(viewModel);
        }

        public ActionResult Close(int id = 0)
        {
            accountService.Close(id);
            return RedirectToAction("Index");
        }

        public ActionResult Debit(int id = 0)
        {
            var account = accountService.Get(id);

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
                accountService.Debit(form.AccountId, form.DebitAmount);
                return RedirectToAction("Index");
            }

            return View("Debit", form);
        }

        public ActionResult Credit(int id = 0)
        {
            var account = accountService.Get(id);

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
                accountService.Credit(form.AccountId, form.CreditAmount);
                return RedirectToAction("Index");
            }
            
            return View("Credit", form);
        }

        public ActionResult IssueBankCard(int id = 0)
        {
            accountService.IssueBankCard(id);
            return RedirectToAction("Index", "BankCard");
        }        
    }
}