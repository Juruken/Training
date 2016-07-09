using System.Collections.Generic;
using System.Linq;
using Kiwiland.Data;
using Kiwiland.Processors;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace KiwilandTests.Processors
{
    [TestFixture]
    public class TripProcessorTests
    {
        private ITripProcessor m_TripProcessor;
        private Dictionary<string, Station> m_StationsByName;

        [SetUp]
        public void Setup()
        {
            SetupStations();

            var stationProvider = new Mock<IStationProvider>();
            stationProvider.Setup(r => r.GetStation(It.IsAny<string>())).Returns((string s) => MockStationProvider(s));

            m_TripProcessor = new TripProcessor(stationProvider.Object);
        }

        // CDC, CEBC, CEBCDC, CDCEBC, CDEBC, CEBCEBC, CEBCEBCEBC
        [TestCase("C", "C", 30, 7, "CDC", 16)]
        public void TestGenerateTrips(string sourceStation, string destinationStation, int maximumDistance, int expectedTripCount, string expectedShortestTripName, int expectedDistance)
        {
            var trips = m_TripProcessor.Process(sourceStation, destinationStation, maximumDistance);
            
            Assert.NotNull(trips);
            Assert.AreEqual(expectedTripCount, trips.Count);

            var shortestTrip = trips.OrderBy(t => t.TotalDistance).First();
            Assert.AreEqual(expectedShortestTripName, shortestTrip.TripName);
            Assert.AreEqual(expectedDistance, shortestTrip.TripName);
        }

        private Station MockStationProvider(string stationName)
        {
            return m_StationsByName[stationName];
        }

        // AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7
        private void SetupStations()
        {
            var stationARoutes = new Dictionary<string, Route>()
            {
                // AB5, AD5, AE7, 
                {"B", new Route { SourceStation = "A", DestinationStation = "B", Distance = 5}},
                {"D", new Route { SourceStation = "A", DestinationStation = "D", Distance = 5}},
                {"E", new Route { SourceStation = "A", DestinationStation = "E", Distance = 7}}
            };

            
            // BC4
            var stationBRoutes = new Dictionary<string, Route>()
            {
                {"C", new Route { SourceStation = "B", DestinationStation = "C", Distance = 4}}
            };
            

            // CD8, CE2
            var stationCRoutes = new Dictionary<string, Route>()
            {
                {"D", new Route { SourceStation = "C", DestinationStation = "D", Distance = 8}},
                {"E", new Route { SourceStation = "C", DestinationStation = "E", Distance = 2}}
            };

            
            // DC8, DE6
            var stationDRoutes = new Dictionary<string, Route>()
            {
                {"C", new Route { SourceStation = "D", DestinationStation = "C", Distance = 8}},
                {"E", new Route { SourceStation = "D", DestinationStation = "E", Distance = 6}}
            };

            // EB3 
            var stationERoutes = new Dictionary<string, Route>()
            {
                {"C", new Route { SourceStation = "E", DestinationStation = "B", Distance = 3}}
            };

            var stationA = new Station { Name = "A", Routes = stationARoutes };
            var stationB = new Station { Name = "B", Routes = stationBRoutes };
            var stationC = new Station { Name = "C", Routes = stationCRoutes };
            var stationD = new Station { Name = "D", Routes = stationDRoutes };
            var stationE = new Station { Name = "E", Routes = stationERoutes };

            m_StationsByName = new Dictionary<string, Station>
            {
                { stationA.Name, stationA },
                { stationB.Name, stationB },
                { stationC.Name, stationC },
                { stationD.Name, stationD },
                { stationE.Name, stationE }
            };
        }
    }
}
