using System;
using System.Linq;
using System.Web.Mvc;

using AsbaBank.Infrastructure;
using AsbaBank.Models;

namespace AsbaBank.Controllers
{
    public class ClientController : Controller
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IRepository<Client> repository;

        public ClientController()
        {
            unitOfWork = MvcApplication.UnitOfWork;
            repository = unitOfWork.GetRepository<Client>();
        }

        public ActionResult Index()
        {
            return View(repository);
        }

        //
        // GET: /Client/Details/5

        public ActionResult Details(int id = 0)
        {
            var client = repository.Get(id);

            if (client == null)
            {
                return HttpNotFound();
            }

            return View(client);
        }

        //
        // GET: /Client/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Client/Create

        [HttpPost]
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    repository.Add(client);
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
                catch 
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return View(client);
        }

        //
        // GET: /Client/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var client = repository.Get(id);

            if (client == null)
            {
                return HttpNotFound();
            }

            return View(client);
        }

        //
        // POST: /Client/Edit/5

        [HttpPost]
        public ActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    repository.Update(client.Id, client);
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return View(client);
        }

        //
        // GET: /Client/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var client = repository.Get(id);

            if (client == null)
            {
                return HttpNotFound();
            }

            return View(client);
        }

        //
        // POST: /Client/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var client = repository.Get(id);
                repository.Remove(client);
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