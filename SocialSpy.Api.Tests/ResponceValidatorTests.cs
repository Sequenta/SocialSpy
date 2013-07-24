using SocialSpy.Api.VK;
using Xunit;

namespace SocialSpy.Api.Tests
{
    public class ResponceValidatorTests
    {
        ResponseValidator validator = new ResponseValidator();

        [Fact]
        public void ReturnsDataWithoutResponseTag()
        {
            var result = validator.RemoveResponseTag("response: [{uid: 1,first_name: 'Павел',last_name: 'Дуров'}]");
            Assert.Equal("{uid: 1,first_name: 'Павел',last_name: 'Дуров'}",result);
        }

        [Fact]
        public void ReturnsValidResponseData()
        {
            var result = validator.GetResponseData("response: [{uid: 1,first_name: 'Павел',last_name: 'Дуров'}]");
            Assert.Equal("1",result["uid"]);
            Assert.Equal("Павел",result["first_name"]);
            Assert.Equal("Дуров",result["last_name"]);
        }

        [Fact]
        public void ThrowsEmptyResponseException()
        {
            Assert.Throws<EmptyResponseException>(() => validator.GetResponseData(""));
        }
    }
}