using System.Web;
using System.Web.Routing;
using Moq;
using NUnit.Framework;

namespace TwilioBarista.Web.Tests
{
    internal class RoutingTest
    {
        private Mock<HttpContextBase> _moqContext;
        private Mock<HttpRequestBase> _moqRequest;
        [SetUp]
        public void SetupTests()
        {
            // Setup Moq
            _moqContext = new Mock<HttpContextBase>();
            _moqRequest = new Mock<HttpRequestBase>();
            _moqContext.Setup(x => x.Request).Returns(_moqRequest.Object);
        }

        [Test]
        public void MoqRoutingTest()
        {
            // Arrange
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            _moqRequest.Setup(e => e.AppRelativeCurrentExecutionFilePath).Returns("~/Home/Index");
            // Act
            var routeData = routes.GetRouteData(_moqContext.Object);
            // Assert
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Home", routeData.Values["controller"]);
            Assert.AreEqual("Index", routeData.Values["action"]);
        }
    }
}
