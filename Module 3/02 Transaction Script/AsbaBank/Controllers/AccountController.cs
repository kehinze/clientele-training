using System.Web.Mvc;
using AsbaBank.Domain;
using AsbaBank.Domain.Models;

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

        //
        // GET: /Account/Details/5

        public ActionResult Details(int id = 0)
        {
            var account = accountService.Get(id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return View(account);
        }

        //
        // GET: /Account/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Account/Create

        [HttpPost]
        public ActionResult Create(Account account)
        {
            if (ModelState.IsValid)
            {
                var newAccount = accountService.Create(account);
                return View("Details", newAccount);
            }

            return View(account);
        }

        //
        // GET: /Account/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var account = accountService.Get(id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return View(account);
        }

        //
        // POST: /Account/Edit/5

        [HttpPost]
        public ActionResult Edit(Account account)
        {
            if (ModelState.IsValid)
            {
                var updatedAccount = accountService.Update(account);
                return View("Details", updatedAccount);
            }

            return View(account);
        }

        public ActionResult Close(int id = 0)
        {
            var account = accountService.Close(id);
            return View("Details", account);
        }
    }
}