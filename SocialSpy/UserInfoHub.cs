using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
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
            Clients.Caller.viewUserInfo(new
            {
                firstName = userInfo.FirstName,
                lastName = userInfo.LastName,
                pictureUrl = userInfo.PictureUrl
            });
        }
    }
}