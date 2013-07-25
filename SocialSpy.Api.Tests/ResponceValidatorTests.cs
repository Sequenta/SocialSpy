using NUnit.Framework;
using SocialSpy.Api.VK;

namespace SocialSpy.Api.Tests
{
    [TestFixture]
    public class ResponceValidatorTests
    {
        ResponseValidator validator = new ResponseValidator();

        [Test]
        public void ReturnsDataWithoutResponseTag()
        {
            var result = validator.RemoveResponseTag("response: [{uid: 1,first_name: 'Павел',last_name: 'Дуров'}]");
            Assert.AreEqual("{uid: 1,first_name: 'Павел',last_name: 'Дуров'}", result);
        }

        [Test]
        public void ReturnsValidResponseData()
        {
            var result = validator.GetResponseData("response: [{uid: 1,first_name: 'Павел',last_name: 'Дуров'}]");
            var res = result["uid"];
            Assert.AreEqual("1", result["uid"].ToString());
            Assert.AreEqual("Павел", result["first_name"].ToString());
            Assert.AreEqual("Дуров", result["last_name"].ToString());
        }

        [Test]
        public void ThrowsEmptyResponseException()
        {
            Assert.Throws<EmptyResponseException>(() => validator.GetResponseData(""));
        }
    }
}