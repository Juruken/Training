using System.Collections.Generic;
using TrainTrip.Data;
using TrainTrip.Processors;
using Moq;
using NUnit.Framework;

namespace TrainTripTests.Providers
{
    [TestFixture]
    public class StationProviderTests
    {
        private StationProvider m_StationProvider;

        private const string INVALID_STATION_NAME = "1";
        
        [SetUp]
        public void SetUp()
        {
            var stationData = new List<string> { "AB3" };
            var routes = new List<Route> { new Route { SourceStation = "A", DestinationStation = "B", Distance = 3} };

            var dataProvider = new Mock<IStationDataProvider>();
            dataProvider.Setup(r => r.GetData()).Returns(stationData);

            var routeProvider = new Mock<IRouteProvider>();
            routeProvider.Setup(r => r.GetRoutes(It.IsAny<string>())).Returns((string s) => s != INVALID_STATION_NAME ? routes : null);

            m_StationProvider = new StationProvider(dataProvider.Object, routeProvider.Object);
        }

        [Test]
        public void TestCreateStation()
        {
            var station = m_StationProvider.GetStation("A");

            Assert.IsNotNull(station);
            Assert.AreEqual("A", station.Name);
            Assert.AreEqual(1, station.Routes.Count);
        }
    }
}
