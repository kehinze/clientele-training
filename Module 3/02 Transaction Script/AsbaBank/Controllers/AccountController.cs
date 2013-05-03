using System;
using System.Web.Mvc;
using AsbaBank.Domain;
using AsbaBank.Domain.Models;
using AsbaBank.Presentation.Mvc.Forms;

namespace AsbaBank.Presentation.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService accountService;

        public AccountController()
        {
            accountService = new AccountService(MvcApplication.UnitOfWork);
        }

        public ActionResult Index()
        {
            return View(accountService.GetAll());
        }

        public ActionResult Details(int id = 0)
        {
            var account = accountService.Get(id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return View(account);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Account account)
        {
            if (ModelState.IsValid)
            {
                accountService.Create(account);
                return RedirectToAction("Index");
            }

            return View(account);
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
            BankCard bankCard = accountService.IssueBankCard(id);
            return RedirectToAction("Details", "BankCard", bankCard);
        }        
    }
}