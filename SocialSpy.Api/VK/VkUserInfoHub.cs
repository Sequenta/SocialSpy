using Microsoft.AspNet.SignalR;

namespace SocialSpy.Api.VK
{
    public class VkUserInfoHub:Hub
    {
        public void GetUserInfo(string id)
        {
            var informer = new VkInformer();
            var info = informer.GetUserInfo(id);
            Clients.Caller.viewUserInfo(info);
        }
    }
}