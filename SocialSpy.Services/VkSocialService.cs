using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using SocialSpy.Domain;

namespace SocialSpy.Services
{
    public class VkSocialService:ISocialService
    {
        public UserInfo GetUserInfo(string user)
        {
            var userInfo = new UserInfo();
            var requestUrl = String.Format("https://api.vk.com/method/getProfiles?uids={0}&fields=photo_200_orig", user);
            var request = WebRequest.Create(new Uri(requestUrl)) as HttpWebRequest;
            var response = (HttpWebResponse)request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                using (var sr = new StreamReader(stream))
                {
                    var userInfoJson = JObject.Parse(sr.ReadToEnd())["response"];
                    userInfo.FirstName = userInfoJson["first_name"].ToString();
                    userInfo.LastName = userInfoJson["last_name"].ToString();
                    userInfo.PictureUrl = userInfoJson["photo_200_orig"].ToString();
                }
            }
            return userInfo;
        }
    }
}