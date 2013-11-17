using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using SocialSpy.Domain;
using SocialSpy.Services;

namespace SocialSpy
{
    public class UserInfoHub:Hub
    { 
        Dictionary<string,ISocialService> services = new Dictionary<string, ISocialService>()
        {
            {"vkontakte",new VkSocialService()},
            {"facebook",new FacebookSocialService()}
        };

        public void GetUserInfo(string user, string socialNetwork)
        {
            var service = services[socialNetwork];
            var userInfo = service.GetUserInfo(user);
            Clients.Caller.viewUserInfo(JObject.FromObject(userInfo).ToString());
        }

        public void GetFriendsInfo(string user, string socialNetwork)
        {
            var service = services[socialNetwork];
            var friendsInfo = service.GetFriendsInfo(user);
            Clients.Caller.viewFriendsInfo(friendsInfo.FriendsList.ToString());
            Clients.Caller.viewFriendsStatistics(new { boys = friendsInfo.Statistic[0], 
                                                       girls = friendsInfo.Statistic[1],
                                                       undefined = friendsInfo.Statistic[2],
                                                       online = friendsInfo.Statistic[3],
                                                       offline = friendsInfo.Statistic[4]
            });
        }
    }
}