using System.Web.Mvc;

namespace SocialSpy.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetUserInfo(string request)
        {
            return Json(request, JsonRequestBehavior.AllowGet);
        }
    }
}
