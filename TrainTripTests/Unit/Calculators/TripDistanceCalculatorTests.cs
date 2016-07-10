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
        private ITripDirectRouteDistanceCalculator m_TripDirectRouteDistanceCalculator;

        [SetUp]
        public void Setup()
        {
            SetupTests();
            m_TripDirectRouteDistanceCalculator = new TripDirectRouteDistanceCalculator(m_StationProvider.Object);
        }
        
        [Test]
        public void TestGetShortestTrip()
        {
            var trip = m_TripDirectRouteDistanceCalculator.GetDirectRouteByLowestDistance("C", "C");

            Assert.NotNull(trip);
            Assert.AreEqual(11, trip.TotalDistance);
            Assert.AreEqual("CEBC", trip.TripName);
        }

        [Test]
        public void TestFailInvalidStationName()
        {           
            Assert.That(() => m_TripDirectRouteDistanceCalculator.GetDirectRouteByLowestDistance("C", "F"), Throws.TypeOf<InvalidStationException>());
        }

        [TestCase("A", "D", "AD", 5)]
        public void TestGetDirectRoute(string sourceStation, string destinationStation, string expectedName, int expectedDistance)
        {
            var trip = m_TripDirectRouteDistanceCalculator.GetDirectRouteByLowestDistance(sourceStation, destinationStation);

            Assert.NotNull(trip);
            Assert.AreEqual(expectedDistance, trip.TotalDistance);
            Assert.AreEqual(expectedName, trip.TripName);
        }
        
        [TestCase("C", "C", "CEBC", 11)]
        public void TestGetNonDirectTrip(string sourceStation, string destinationStation, string expectedShortestTripName, int expectedDistance)
        {
            var trip = m_TripDirectRouteDistanceCalculator.GetDirectRouteByLowestDistance(sourceStation, destinationStation);
            
            Assert.NotNull(trip);
            Assert.AreEqual(expectedShortestTripName, trip.TripName);
            Assert.AreEqual(expectedDistance, trip.TotalDistance);
        }

        [TestCase("A", "C")]
        [TestCase("C", "C")]
        public void TestFailDirectRoute(string sourceStation, string destinationStation)
        {
            var trip = m_TripDirectRouteDistanceCalculator.GetDirectRouteByLowestDistance(sourceStation, destinationStation);

            Assert.IsNull(trip);
        }

        [TestCase("C", "C")]
        public void TestFailMaximumDistance(string sourceStation, string destinationStation)
        {
            var trips = m_TripDirectRouteDistanceCalculator.GetDirectRouteByLowestDistance(sourceStation, destinationStation);

            Assert.IsNull(trips); 
        }
    }
}
