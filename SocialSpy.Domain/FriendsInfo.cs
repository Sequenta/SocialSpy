using Newtonsoft.Json.Linq;

namespace SocialSpy.Domain
{
    public class FriendsInfo
    {
        public JToken FriendsList { get; set; } 
        public int[] Statistic { get; set; } 
    }
}