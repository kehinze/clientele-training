using System.Linq;
using System.Web.Mvc;

using AsbaBank.Infrastructure;
using AsbaBank.Models;

namespace AsbaBank.Controllers
{
    public class BankCardController : Controller
    {
        readonly IRepository repository = MvcApplication.Repository;

        public ActionResult Index()
        {
            return View(repository.All<BankCard>().ToList());
        }

        //
        // GET: /BankCard/Details/5

        public ActionResult Details(int id = 0)
        {
            var bankCard = repository.Get<BankCard>(id);

            if (bankCard == null)
            {
                return HttpNotFound();
            }

            return View(bankCard);
        }

        //
        // GET: /BankCard/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BankCard/Create

        [HttpPost]
        public ActionResult Create(BankCard bankCard)
        {
            if (ModelState.IsValid)
            {
                repository.Add(bankCard);
                return RedirectToAction("Index");
            }

            return View(bankCard);
        }

        //
        // GET: /BankCard/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var bankCard = repository.Get<BankCard>(id);

            if (bankCard == null)
            {
                return HttpNotFound();
            }

            return View(bankCard);
        }

        //
        // POST: /BankCard/Edit/5

        [HttpPost]
        public ActionResult Edit(BankCard bankCard)
        {
            if (ModelState.IsValid)
            {
                repository.Update(bankCard.Id, bankCard);
                return RedirectToAction("Index");
            }

            return View(bankCard);
        }

        //
        // GET: /BankCard/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var bankCard = repository.Get<BankCard>(id);

            if (bankCard == null)
            {
                return HttpNotFound();
            }

            return View(bankCard);
        }

        //
        // POST: /BankCard/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var bankCard = repository.Get<BankCard>(id);
            repository.Remove(bankCard);
            return RedirectToAction("Index");
        }
    }
}