using System.Web.Mvc;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;
using AsbaBank.Presentation.Mvc.Forms;

namespace AsbaBank.Presentation.Mvc.Controllers
{
    public class ClientController : Controller
    {
        private readonly IRepository<Client> clientRepository;
        private readonly IUnitOfWork unitOfWork;

        public ClientController()
        {
            unitOfWork = MvcApplication.UnitOfWork;
            clientRepository = unitOfWork.GetRepository<Client>();
        }

        public ActionResult Index()
        {
            return View(clientRepository);
        }

        public ActionResult Details(int id = 0)
        {
            var client = clientRepository.Get(id);

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
        public ActionResult Create(NewClientForm clientForm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = new Client(clientForm.ClientName, clientForm.PhoneNumber);
                    clientRepository.Add(client);
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return View(clientForm);
        }

        public ActionResult OpenAccount(int id)
        {
            return RedirectToAction("OpenAccount", "Account", new { clientId = id });
        }
    }
}