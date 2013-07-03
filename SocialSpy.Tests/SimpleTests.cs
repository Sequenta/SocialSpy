using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocialSpy.Controllers;

namespace SocialSpy.Tests
{
    [TestClass]
    public class SimpleTests
    {
        [TestMethod]
        public void HomeController_ReturnsCorrectValue()
        {
            var controller = new HomeController();

            var value = controller.Index();

            Assert.AreEqual("HelloWorld!",value);
        }
    }
}