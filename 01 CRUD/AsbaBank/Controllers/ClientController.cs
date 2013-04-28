using System.Linq;
using System.Web.Mvc;

using AsbaBank.Infrastructure;
using AsbaBank.Models;

namespace AsbaBank.Controllers
{
    public class ClientController : Controller
    {
        readonly IRepository repository = MvcApplication.Repository;

        public ActionResult Index()
        {
            return View(repository.All<Client>().ToList());
        }

        //
        // GET: /Client/Details/5

        public ActionResult Details(int id = 0)
        {
            var client = repository.Get<Client>(id);

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
                repository.Add(client);
                return RedirectToAction("Index");
            }

            return View(client);
        }

        //
        // GET: /Client/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var client = repository.Get<Client>(id);

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
                repository.Update(client.Id, client);
                return RedirectToAction("Index");
            }

            return View(client);
        }

        //
        // GET: /Client/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var client = repository.Get<Client>(id);

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
            var client = repository.Get<Client>(id);
            repository.Remove(client);
            return RedirectToAction("Index");
        }
    }    
}