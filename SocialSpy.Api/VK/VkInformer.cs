using System.IO;
using Microsoft.AspNet.SignalR.Client.Http;

namespace SocialSpy.Api.VK
{
    public class VkInformer
    {
        public string GetUserInfo(string id)
        {
            var http = new DefaultHttpClient();
            var requestUrl = string.Format("https://api.vk.com/method/getProfiles?uids={0}&fields=sex,city,country,photo_200_orig", id);
            string userInfo;
            using (var result = http.Post(requestUrl, request => { }).Result)
            {
                using (var resultStream = new StreamReader(result.GetStream()))
                {
                    userInfo = resultStream.ReadToEnd();
                }
            }
            return userInfo;
        } 
    }
}