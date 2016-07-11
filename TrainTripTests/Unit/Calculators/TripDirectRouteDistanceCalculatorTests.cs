using System.Linq;
using TrainTrip.Exceptions;
using TrainTrip.Processors;
using NUnit.Framework;
using TrainTripTests.Calculators;

namespace TrainTripTests.Processors
{
    [TestFixture]
    public class TripDirectRouteDistanceCalculatorTests : BaseCalculatorTests
    {
        private ITripDirectRouteDistanceCalculator m_TripDirectRouteDistanceCalculator;

        [SetUp]
        public void Setup()
        {
            SetupTests();
            m_TripDirectRouteDistanceCalculator = new TripDirectRouteDistanceCalculator(m_StationProvider.Object);
        }

        [Test]
        public void TestFailInvalidStationName()
        {           
            Assert.That(() => m_TripDirectRouteDistanceCalculator.GetDirectRouteByLowestDistanceWithoutRecursion("C", "F"), Throws.TypeOf<InvalidStationException>());
        }

        // A Routes
        [TestCase("A", "B", "AB", 5)]
        [TestCase("A", "C", "ABC", 9)]
        [TestCase("A", "D", "AD", 5)]
        [TestCase("A", "E", "AE", 7)]
        // B Routes
        [TestCase("B", "B", "BCEB", 9)]
        [TestCase("B", "C", "BC", 4)]
        [TestCase("B", "D", "BCD", 12)]
        [TestCase("B", "E", "BCE", 6)]
        // C Routes
        [TestCase("C", "B", "CEB", 5)]
        [TestCase("C", "C", "CEBC", 9)]
        [TestCase("C", "D", "CD", 8)]
        [TestCase("C", "E", "CE", 2)]
        // D Routes
        [TestCase("D", "B", "DEB", 9)]
        [TestCase("D", "C", "DC", 8)]
        [TestCase("D", "D", "DCD", 16)]
        [TestCase("D", "E", "DE", 6)]
        // E Routes
        [TestCase("E", "B", "EB", 3)]
        [TestCase("E", "C", "EBC", 7)]
        [TestCase("E", "D", "EBCD", 15)]
        [TestCase("E", "E", "EBCE", 9)]
        public void TestGetDirectRouteByLowestDistanceWithRecursion(string sourceStation, string destinationStation, string expectedShortestTripName, int expectedDistance)
        {
            var trip = m_TripDirectRouteDistanceCalculator.GetDirectRouteByLowestDistanceWithRecursion(sourceStation, destinationStation);
            
            Assert.NotNull(trip);
            Assert.AreEqual(expectedDistance, trip.TotalDistance);
            Assert.AreEqual(expectedShortestTripName, trip.TripName);
        }

        // A Routes 
        [TestCase("A", "A")]
        // B Routes
        [TestCase("B", "A")]
        // C Routes
        [TestCase("C", "A")]
        // D Routes
        [TestCase("D", "A")]
        // E Routes
        [TestCase("E", "A")]
        public void TestGetFailToGetDirectRouteByLowestDistanceWithRecursion(string sourceStation, string destinationStation)
        {
            var trip = m_TripDirectRouteDistanceCalculator.GetDirectRouteByLowestDistanceWithRecursion(sourceStation, destinationStation);

            Assert.IsNull(trip);
        }

        // A Routes
        [TestCase("A", "B", "AB", 5)]
        [TestCase("A", "D", "AD", 5)]
        [TestCase("A", "E", "AE", 7)]
        // B Routes
        [TestCase("B", "C", "BC", 4)]
        // C Routes
        [TestCase("C", "D", "CD", 8)]
        [TestCase("C", "E", "CE", 2)]
        // D Routes
        [TestCase("D", "C", "DC", 8)]
        [TestCase("D", "E", "DE", 6)]
        // E Routes
        [TestCase("E", "B", "EB", 3)]
        public void TestGetDirectRouteByLowestDistanceWithoutRecursion(string sourceStation, string destinationStation, string expectedShortestTripName, int expectedDistance)
        {
            var trip = m_TripDirectRouteDistanceCalculator.GetDirectRouteByLowestDistanceWithoutRecursion(sourceStation, destinationStation);

            Assert.NotNull(trip);
            Assert.AreEqual(expectedDistance, trip.TotalDistance);
            Assert.AreEqual(expectedShortestTripName, trip.TripName);
        }

        // A routes
        [TestCase("A", "A")]
        [TestCase("A", "C")]
        // B Routes
        [TestCase("B", "A")]
        [TestCase("B", "B")]
        [TestCase("B", "D")]
        [TestCase("B", "E")]
        // C Routes
        [TestCase("C", "A")]
        [TestCase("C", "B")]
        [TestCase("C", "C")]
        // D Routes
        [TestCase("D", "A")]
        [TestCase("D", "B")]
        [TestCase("D", "D")]
        // E Routes
        [TestCase("E", "A")]
        [TestCase("E", "C")]
        [TestCase("E", "D")]
        [TestCase("E", "E")]
        public void TestFailToGetTripWithoutRecursion(string sourceStation, string destinationStation)
        {
            var trip = m_TripDirectRouteDistanceCalculator.GetDirectRouteByLowestDistanceWithoutRecursion(sourceStation, destinationStation);

            Assert.IsNull(trip);
        }
    }
}

