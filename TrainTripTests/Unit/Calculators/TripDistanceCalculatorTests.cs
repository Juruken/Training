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
            var trip = m_TripDistanceCalculator.GetFastestTripByDistance("C", "C", 30, false);

            Assert.NotNull(trip);
            Assert.AreEqual(11, trip.TotalDistance);
            Assert.AreEqual("CEBC", trip.TripName);
        }

        [Test]
        public void TestFailInvalidStationName()
        {           
            Assert.That(() => m_TripDistanceCalculator.GetFastestTripByDistance("C", "F", 30, true), Throws.TypeOf<InvalidStationException>());
        }

        [TestCase("A", "D", 30, true, "AD", 5)]
        public void TestGetDirectRoute(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly, string expectedName, int expectedDistance)
        {
            var trip = m_TripDistanceCalculator.GetFastestTripByDistance(sourceStation, destinationStation, maximumDistance, directRouteOnly);

            Assert.NotNull(trip);
            Assert.AreEqual(expectedDistance, trip.TotalDistance);
            Assert.AreEqual(expectedName, trip.TripName);
        }
        
        [TestCase("C", "C", 30, false, 2, "CEBC", 11)]
        public void TestGetNonDirectTrip(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly,
            int expectedTripCount, string expectedShortestTripName, int expectedDistance)
        {
            var trips = m_TripDistanceCalculator.GetTripsByDistance(sourceStation, destinationStation, maximumDistance, directRouteOnly);
            
            Assert.NotNull(trips);
            Assert.AreEqual(expectedTripCount, trips.Count);
            
            var shortestTrip = trips.OrderBy(t => t.TotalDistance).First();
            Assert.AreEqual(expectedShortestTripName, shortestTrip.TripName);
            Assert.AreEqual(expectedDistance, shortestTrip.TotalDistance);
        }

        [TestCase("A", "C", 30, true)]
        [TestCase("C", "C", 30, true)]
        public void TestFailDirectRoute(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly)
        {
            var trips = m_TripDistanceCalculator.GetTripsByDistance(sourceStation, destinationStation, maximumDistance, directRouteOnly);

            Assert.IsNull(trips);
        }

        [TestCase("C", "C", 1, false)]
        public void TestFailMaximumDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly)
        {
            var trips = m_TripDistanceCalculator.GetTripsByDistance(sourceStation, destinationStation, maximumDistance, directRouteOnly);

            Assert.IsNull(trips); 
        }
    }
}
