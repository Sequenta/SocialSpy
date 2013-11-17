using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
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
                    var userInfoJson = JObject.Parse(sr.ReadToEnd())["response"].First;
                    userInfo = JsonConvert.DeserializeObject<UserInfo>(userInfoJson.ToString());
                }
            }
            return userInfo;
        }

        public FriendsInfo GetFriendsInfo(string user)
        {
            var friendsInfo = new FriendsInfo();
            var requestUrl = String.Format("https://api.vk.com/method/friends.get?user_id={0}&fields=photo_200_orig,online,sex&count=10", user);
            var request = WebRequest.Create(new Uri(requestUrl)) as HttpWebRequest;
            var response = (HttpWebResponse)request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                using (var sr = new StreamReader(stream))
                {
                    var friendsInfoJson = JObject.Parse(sr.ReadToEnd())["response"];
                    friendsInfo.FriendsList = friendsInfoJson;
                    friendsInfo.Statistic = GetFriendsStatistic(friendsInfoJson);
                }
            }
            return friendsInfo;
        }

        private int[] GetFriendsStatistic(JToken friendsInfoJson)
        {
            var online = 0;
            var offline = 0;
            var boys = 0;
            var girls = 0;
            var undefined = 0;
            foreach (var friend in friendsInfoJson)
            {
                var friendObject = JsonConvert.DeserializeObject<UserInfo>(friend.ToString());
                switch (friendObject.Sex)
                {
                    case "1":
                        girls += 1;
                        break;
                    case "2":
                        boys += 1;
                        break;
                    default:
                        undefined += 1;
                        break;
                }
                if (friendObject.IsOnline == "1")
                {
                    online += 1;
                }
                else
                {
                    offline += 1;
                }
            }

            return new[] {boys, girls, undefined, online, offline};
        }
    }
}