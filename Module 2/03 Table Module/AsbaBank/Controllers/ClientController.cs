using System;
using System.Web.Mvc;
using AsbaBank.Domain;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;

namespace AsbaBank.Presentation.Mvc.Controllers
{
    public class ClientController : Controller
    {
        private readonly ClientModule clientModule;
        private readonly IUnitOfWork unitOfWork;

        public ClientController()
        {
            unitOfWork = MvcApplication.UnitOfWork;
            clientModule = new ClientModule(unitOfWork.GetRepository<Client>());
        }

        public ActionResult Index()
        {
            return View(clientModule.GetAll());
        }

        public ActionResult Details(int id = 0)
        {
            var client = clientModule.Get(id);

            if (client == null)
            {
                return HttpNotFound();
            }

            return View(client);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    clientModule.Create(client);
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

        public ActionResult Edit(int id = 0)
        {
            var client = clientModule.Get(id);

            if (client == null)
            {
                return HttpNotFound();
            }

            return View(client);
        }

        [HttpPost]
        public ActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    clientModule.Update(client);
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

       public ActionResult OpenAccount(int id)
       {
           return RedirectToAction("OpenAccount", "Account", new { clientId = id });
       }
    }    
}