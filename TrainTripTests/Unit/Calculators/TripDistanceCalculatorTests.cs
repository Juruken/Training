using System;
using System.Linq;
using TrainTrip.Exceptions;
using TrainTrip.Processors;
using NUnit.Framework;
using TrainTripTests.Calculators;

namespace TrainTripTests.Processors
{
    [TestFixture]
    public class TripDistanceCalculatorTests : BaseCalculatorTests
    {
        private ITripDistanceCalculator m_TripDistanceCalculator;

        [SetUp]
        public void Setup()
        {
            SetupTests();
            m_TripDistanceCalculator = new TripDistanceCalculator(m_StationProvider.Object);
        }
        
        [Test]
        public void TestGetShortestTrip()
        {
            var trip = m_TripDistanceCalculator.GetFastestTripByDistance("C", "C");

            Assert.NotNull(trip);
            Assert.AreEqual(11, trip.TotalDistance);
            Assert.AreEqual("CEBC", trip.TripName);
        }

        [Test]
        public void TestFailToGetInvalidTrip()
        {           
            Assert.That(() => m_TripDistanceCalculator.GetFastestTripByDistance("C", "F"), Throws.TypeOf<InvalidStationException>());
        }

        [TestCase("C", "C", 30, 2, "CEBC", 11)]
        public void TestGenerateTrips(string sourceStation, string destinationStation, int maximumDistance, int expectedTripCount, string expectedShortestTripName, int expectedDistance)
        {
            var trips = m_TripDistanceCalculator.GetTripsByDistance(sourceStation, destinationStation, maximumDistance);
            
            Assert.NotNull(trips);
            Assert.AreEqual(expectedTripCount, trips.Count);

            var shortestTrip = trips.OrderBy(t => t.TotalDistance).First();
            Assert.AreEqual(expectedShortestTripName, shortestTrip.TripName);
            Assert.AreEqual(expectedDistance, shortestTrip.TotalDistance);
        }
    }
}
