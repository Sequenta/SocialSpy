using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Json;
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
        public ViewResult GetUserInfo(string id)
        {
            var http = new DefaultHttpClient();
            var requestUrl = string.Format("https://api.vk.com/method/getProfiles?uids={0}&fields=sex,city,country,photo_200_orig", id);
            UserInfo userInfo;
            using (var result = http.Post(requestUrl, request => { }).Result)
            {
                using (var resultStream = new StreamReader(result.GetStream()))
                {
                    var jsonResult = resultStream.ReadToEnd();
                    userInfo =  CreateUserInfo(jsonResult);
                }
            }
            return View("UserInfo",userInfo);
        }

        private UserInfo CreateUserInfo(string jsonResult)
        {
            jsonResult = jsonResult.Replace(@"{""response"":[", "");
            jsonResult = jsonResult.Remove(jsonResult.Length - 2);
            var serializer = new JsonNetSerializer();
            var values = serializer.Parse<Dictionary<string, string>>(jsonResult);

            var userInfo = new UserInfo();
            userInfo.UserId = values["uid"];
            userInfo.FirstName = values["first_name"];
            userInfo.LastName = values["last_name"];
            userInfo.Sex = TryGetValue("sex",values);
            userInfo.City = TryGetValue("city", values);
            userInfo.Country = TryGetValue("country", values);
            userInfo.Image = TryGetValue("photo_200_orig", values);
            return userInfo;
        }

        private string TryGetValue(string valueName, Dictionary<string, string> values)
        {
            string outValue;
            values.TryGetValue(valueName, out outValue);
            return outValue;
        }
    }
}
