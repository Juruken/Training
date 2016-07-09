using System.Collections.Generic;
using System.Linq;
using Kiwiland.Processors;
using Kiwiland.Providers;
using Moq;
using NUnit.Framework;

namespace KiwilandTests.Providers
{
    [TestFixture]
    public class RouteProviderTests
    {
        private IRouteProvider m_RouteProvider;

        [SetUp]
        public void SetUp()
        {
            var dataProvider = new Mock<IRouteDataProvider>();
            dataProvider.Setup(r => r.GetData()).Returns(new List<string> {"AB1"});

            m_RouteProvider = new RouteProvider(dataProvider.Object);
        }

        [Test]
        public void TestGetRoute()
        {
            var routes = m_RouteProvider.GetRoutes("A");

            Assert.IsNotNull(routes);
            Assert.AreEqual(1, routes.Count);

            var route = routes.First();

            Assert.AreEqual("A", route.SourceStation);
            Assert.AreEqual("B", route.DestinationStation);
            Assert.AreEqual(1, route.Distance);
        }
    }
}
