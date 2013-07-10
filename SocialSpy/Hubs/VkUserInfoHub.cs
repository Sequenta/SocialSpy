using Microsoft.AspNet.SignalR;
using SocialSpy.Informers;

namespace SocialSpy.Hubs
{
    public class VkUserInfoHub:Hub
    {
        public void GetUserInfo(string id)
        {
            var informer = new VkInformer();
            var info = informer.GetUserInfo(id);
            Clients.All.viewUserInfo(info);
        }
    }
}