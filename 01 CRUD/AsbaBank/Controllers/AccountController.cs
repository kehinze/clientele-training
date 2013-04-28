using System.Data;
using System.Linq;
using System.Web.Mvc;

using AsbaBank.Infrastructure;
using AsbaBank.Models;

namespace AsbaBank.Controllers
{
    public class AccountController : Controller
    {
        readonly IRepository repository = MvcApplication.Repository;

        public ActionResult Index()
        {
            return View(repository.All<Account>().ToList());
        }

        //
        // GET: /Account/Details/5

        public ActionResult Details(int id = 0)
        {
            var account = repository.Get<Account>(id);

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
                repository.Add(account);
                return RedirectToAction("Index");
            }

            return View(account);
        }

        //
        // GET: /Account/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var account = repository.Get<Account>(id);

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
                repository.Update(account.Id, account);
                return RedirectToAction("Index");
            }

            return View(account);
        }

        //
        // GET: /Account/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var account = repository.Get<Account>(id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return View(account);
        }

        //
        // POST: /Account/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var account = repository.Get<Account>(id);
            repository.Remove(account);
            return RedirectToAction("Index");
        }
    }
}