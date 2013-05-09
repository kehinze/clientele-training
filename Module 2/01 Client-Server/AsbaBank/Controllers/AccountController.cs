using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

using AsbaBank.Infrastructure;
using AsbaBank.Models;

namespace AsbaBank.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IRepository<Account> repository;

        public AccountController()
        {
            unitOfWork = new InMemoryUnitOfWork(MvcApplication.DataStore);
            repository = unitOfWork.GetRepository<Account>();
        }

        public ActionResult Index()
        {
            return View(repository);
        }

        //
        // GET: /Account/Details/5

        public ActionResult Details(int id = 0)
        {
            var account = repository.Get(id);

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
                try
                {
                    repository.Add(account);
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return View(account);
        }

        //
        // GET: /Account/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var account = repository.Get(id);

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
                try
                {
                    repository.Update(account.Id, account);
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return View(account);
        }

        //
        // GET: /Account/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var account = repository.Get(id);

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
            try
            {
                var account = repository.Get(id);
                repository.Remove(account);
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