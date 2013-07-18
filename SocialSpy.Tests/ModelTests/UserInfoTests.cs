using SocialSpy.Models;
using Xunit;

namespace SocialSpy.Tests.ModelTests
{
    public class UserInfoTests
    {
        [Fact]
        public void UserInfo_ReturnsEmptyInstance_WhenJsonStringIsEmpty()
        {
            var jsonString = "";
            var userInfo = new UserInfo(jsonString);

            Assert.Equal(new UserInfo(),userInfo);
        } 
    }
}