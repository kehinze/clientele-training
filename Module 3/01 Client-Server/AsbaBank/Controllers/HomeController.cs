using System.Web.Mvc;

namespace AsbaBank.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Index", "Client");
        }
    }
}
