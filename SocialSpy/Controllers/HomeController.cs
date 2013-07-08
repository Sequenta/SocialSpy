using System.Web.Mvc;
using SocialSpy.Models;

namespace SocialSpy.Controllers
{
    public class HomeController : Controller
    {
        UserInfo userInfo = new UserInfo();
        public ViewResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetUserInfo(string request)
        {
            var info = userInfo.GetUserInfo(request);
            return Json(info, JsonRequestBehavior.AllowGet);
        }
    }
}
