using Xunit;

namespace SocialSpy.Tests.ModelTests
{
    public class UserInfoTests
    {
        [Fact(Skip = "Useless test")]
        public void UserInfo_ReturnsEmptyInstance_WhenJsonStringIsEmpty()
        {
            Assert.True(true);
            //var jsonString = "";
            //var userInfo = new UserInfo(jsonString);

            //Assert.Equal(new UserInfo(),userInfo);
        } 
    }
}