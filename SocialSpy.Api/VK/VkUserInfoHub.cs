using Microsoft.AspNet.SignalR;

namespace SocialSpy.Api.VK
{
    public class VkUserInfoHub:Hub
    {
        private VkInformer informer = new VkInformer();

        public void GetUserInfo(string id)
        {
            var info = informer.GetUserInfo(id);
            Clients.Caller.viewUserInfo(info);
        }

        public void GetFriendsInfo(string id)
        {
            informer.GetFriendsInfo(id);
        }

        public void ShowFriendInfo(string friendInfo)
        {
            Clients.All.viewFriendsInfo(friendInfo);
        }

        public void ShowUserStatistic(string statistic)
        {
            Clients.All.viewUserStatistic(statistic);
        }
    }
}