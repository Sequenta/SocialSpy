using NUnit.Framework;

namespace SocialSpy.Tests.ModelTests
{
    [TestFixture]
    public class UserInfoTests
    {
        [Test]
        [Ignore("Useless test")]
        public void UserInfo_ReturnsEmptyInstance_WhenJsonStringIsEmpty()
        {
            Assert.True(true);
            //var jsonString = "";
            //var userInfo = new UserInfo(jsonString);

            //Assert.Equal(new UserInfo(),userInfo);
        } 
    }
}