using System.Web.Mvc;
using SocialSpy.Models;

namespace SocialSpy.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }

        [HttpPost]
        public ViewResult GetUserInfoView(string jsonUserInfo)
        {
            var userInfo = new UserInfo(jsonUserInfo);
            return View("UserInfo", userInfo);
        }

        [HttpPost]
        public ActionResult GetFriendsInfoView(string jsonUserInfo)
        {
            var userInfo = new UserInfo(jsonUserInfo);
            return View("FriendsView", userInfo);
        }
    }
}
