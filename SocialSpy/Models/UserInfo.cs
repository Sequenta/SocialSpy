using SocialSpy.Api.VK;
using SocialSpy.Service;

namespace SocialSpy.Models
{
    public class UserInfo
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }

        public UserInfo(string jsonUserInfo)
        {
            var validator = new ResponseValidator();
            var userInfo = validator.GetResponseData(jsonUserInfo);
            UserId = userInfo["uid"].ToString();
            FirstName = userInfo["first_name"].ToString();
            LastName = userInfo["last_name"].ToString();
            Image = userInfo["photo_200_orig"].ToString();
        }
    }
}