using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using SocialSpy.Domain;

namespace SocialSpy.Services
{
    public class FacebookSocialService:ISocialService
    {
        public UserInfo GetUserInfo(string user)
        {
            var userInfo = new UserInfo();
            var requestUrl = String.Format("https://graph.facebook.com/{0}", user);
            var request = WebRequest.Create(new Uri(requestUrl)) as HttpWebRequest;
            var response = (HttpWebResponse)request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                using (var sr = new StreamReader(stream))
                {
                    var userInfoJson = JObject.Parse(sr.ReadToEnd());
                    userInfo.FirstName = userInfoJson["first_name"].ToString();
                    userInfo.LastName = userInfoJson["last_name"].ToString();
                }
            }
            userInfo.PictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type=large", user);
            return userInfo;
        }
    }
}