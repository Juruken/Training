using System;
using System.Collections.Generic;
using TrainTrip.Calculators;
using TrainTrip.Data;
using TrainTrip.Processors;
using Moq;
using NUnit.Framework;

namespace TrainTripTests.Calculators
{
    [TestFixture]
    public class JourneyCalculatorTests : BaseCalculatorTests
    {
        private IJourneyCalculator m_JourneyCalculator;
        private Dictionary<Tuple<string,string>, Trip> m_TripsBySourceAndDestination;

        [SetUp]
        public void Setup()
        {
            SetupTests();

            CreateTrips();

            var tripCalculator = new Mock<ITripDistanceCalculator>();
            tripCalculator.Setup(r => r.GetFastestTripByDistance(It.IsAny<string>(), It.IsAny<string>())).Returns((string s, string t) => GetTrip(s, t));
            
            m_JourneyCalculator = new JourneyCalculator(m_StationProvider.Object, tripCalculator.Object);
        }

        [Test]
        // TODO: Add more tests cases, using test case return function
        public void TestGenerateJourney()
        {
            // A - B - C
            var stations = new[] {"A", "B", "C"};
            var journey = m_JourneyCalculator.Calculate(stations);

            Assert.NotNull(journey);
            Assert.AreEqual(9, journey.Distance);
        }

        private Trip GetTrip(string sourceStation, string destinationStation)
        {
            var tripKey = new Tuple<string, string>(sourceStation, destinationStation);

            return m_TripsBySourceAndDestination.ContainsKey(tripKey) ? m_TripsBySourceAndDestination[tripKey] : null;
        }

        private void CreateTrips()
        {
            m_TripsBySourceAndDestination = new Dictionary<Tuple<string, string>, Trip>
            {
                // A -> B
                // AB5
                { new Tuple<string, string>("A", "B"),
                    new Trip
                    {
                        TripName = "AB",
                        TotalDistance = 5
                    }
                },
                // B -> C
                // BC4
                { new Tuple<string, string>("B", "C"),
                    new Trip
                    {
                        TripName = "BC",
                        TotalDistance = 4
                    }
                }
            };
        }
    }
}
