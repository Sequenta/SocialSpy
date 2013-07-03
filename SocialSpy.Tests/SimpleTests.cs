using NUnit.Framework;
using SocialSpy.Controllers;

namespace SocialSpy.Tests
{
    [TestFixture]
    public class SimpleTests
    {
        [Test]
        public void HomeController_ReturnsCorrectValue()
        {
            var controller = new HomeController();

            var value = controller.Index();

            Assert.AreEqual("HelloWorld!",value);
        }
    }
}