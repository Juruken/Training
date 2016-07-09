using System.Collections.Generic;
using Kiwiland.Data;
using Kiwiland.Processors;
using Moq;
using NUnit.Framework;

namespace KiwilandTests.Calculators
{
    [TestFixture()]
    public class BaseCalculatorTests
    {
        protected Mock<IStationProvider> m_StationProvider;
        
        private Dictionary<string, Station> m_StationsByName;

        [SetUp]
        public void Setup()
        {
            SetupStations();

            m_StationProvider = new Mock<IStationProvider>();
            m_StationProvider.Setup(r => r.GetStation(It.IsAny<string>())).Returns((string s) => MockStationProvider(s));
        }

        private Station MockStationProvider(string stationName)
        {
            return m_StationsByName.ContainsKey(stationName) ? m_StationsByName[stationName] : null;
        }

        // Test Routes: AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7
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
                {"B", new Route { SourceStation = "E", DestinationStation = "B", Distance = 3}}
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
