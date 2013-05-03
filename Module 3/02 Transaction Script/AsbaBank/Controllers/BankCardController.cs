using System.Web.Mvc;
using AsbaBank.Domain;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;

namespace AsbaBank.Presentation.Mvc.Controllers
{
    public class BankCardController : Controller
    {
        private readonly BankCardService bankCardService;

        public BankCardController()
        {
            bankCardService = new BankCardService(MvcApplication.UnitOfWork);
        }

        public ActionResult Index()
        {
            return View(bankCardService.GetAll());
        }

        //
        // GET: /BankCard/Details/5

        public ActionResult Details(int id = 0)
        {
            var bankCard = bankCardService.Get(id);

            if (bankCard == null)
            {
                return HttpNotFound();
            }

            return View(bankCard);
        }

        public ActionResult Disable(int id = 0)
        {
            BankCard bankCard = bankCardService.Disable(id);
            return View("Details", bankCard);
        }

        public ActionResult IssueBankCard(int id = 0, decimal amount = 0)
        {
            Account account = bankCardService.Withdraw(id, amount);
            return RedirectToAction("Details", "Account", account);
        }
    }
}