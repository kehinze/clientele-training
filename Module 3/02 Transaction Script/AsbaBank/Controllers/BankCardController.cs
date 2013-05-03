using System.Web.Mvc;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;

namespace AsbaBank.Presentation.Mvc.Controllers
{
    public class BankCardController : Controller
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IRepository<BankCard> repository;

        public BankCardController()
        {
            unitOfWork = MvcApplication.UnitOfWork;
            repository = unitOfWork.GetRepository<BankCard>();
        }

        public ActionResult Index()
        {
            return View(repository);
        }

        //
        // GET: /BankCard/Details/5

        public ActionResult Details(int id = 0)
        {
            var bankCard = repository.Get(id);

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
                try
                {
                    repository.Add(bankCard);
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return View(bankCard);
        }

        //
        // GET: /BankCard/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var bankCard = repository.Get(id);

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
                try
                {
                    repository.Update(bankCard.Id, bankCard);
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return View(bankCard);
        }

        //
        // GET: /BankCard/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var bankCard = repository.Get(id);

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
            try
            {
                var bankCard = repository.Get(id);
                repository.Remove(bankCard);
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