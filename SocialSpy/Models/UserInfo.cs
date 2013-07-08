using System.IO;
using Microsoft.AspNet.SignalR.Client.Http;

namespace SocialSpy.Models
{
    public class UserInfo
    {
        public string GetUserInfo(string id)
        {
            var http = new DefaultHttpClient();
            var requestUrl = string.Format("https://api.vk.com/method/getProfiles?uids={0}&fields=sex,city,country", id);
            var jsonResult = "";
            using (var result = http.Post(requestUrl, request => { }).Result)
            {
                using (var resultStream = new StreamReader(result.GetStream()))
                {
                    jsonResult = resultStream.ReadToEnd();
                }
            }
            return jsonResult;
        } 
    }
}