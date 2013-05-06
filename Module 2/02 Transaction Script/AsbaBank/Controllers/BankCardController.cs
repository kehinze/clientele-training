using System.Collections.Generic;
using System.Web.Mvc;
using AsbaBank.Domain;
using AsbaBank.Presentation.Mvc.ViewModelBuilders;
using AsbaBank.Presentation.Mvc.ViewModels;

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
            IEnumerable<BankCardViewModel> viewModel = BankCardViewModelBuilder.Build(bankCardService);
            return View(viewModel);
        }

        public ActionResult Disable(int id = 0)
        {
            bankCardService.Disable(id);
            return RedirectToAction("Index");
        }
    }
}