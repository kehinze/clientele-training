using System.Web.Mvc;
using AsbaBank.Domain;
using AsbaBank.Domain.Models;

namespace AsbaBank.Presentation.Mvc.Controllers
{
    public class ClientController : Controller
    {
        private readonly ClientService clientService;

        public ClientController()
        {
            clientService = new ClientService(MvcApplication.UnitOfWork);
        }

        public ActionResult Index()
        {
            return View(clientService.GetAll());
        }

        public ActionResult Details(int id = 0)
        {
            var client = clientService.Get(id);

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
                clientService.Create(client);
                return RedirectToAction("Index");
            }

            return View(client);
        }

        public ActionResult Edit(int id = 0)
        {
            var client = clientService.Get(id);

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
                clientService.Update(client);
                return RedirectToAction("Index");
            }

            return View(client);
        }

       public ActionResult OpenAccount(int id)
       {
           clientService.OpenAccount(id);
           return RedirectToAction("Index", "Account");
       }
    }    
}