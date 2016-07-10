using TrainTrip.Calculators;
using NUnit.Framework;
using TrainTrip.Exceptions;
using TrainTripTests.Calculators;

namespace TrainTripTests.Unit.Calculators
{
    [TestFixture]
    public class TripStopCalculatorTests : BaseCalculatorTests
    {
        private ITripStopCalculator m_TripStopCalculator;

        [SetUp]
        public void Setup()
        {
            SetupTests();
            
            m_TripStopCalculator = new TripStopCalculator(m_StationProvider.Object);
        }
        
        [TestCase("C", "F", 5)]
        public void TestInvalidRouteThrowsException(string sourceStation, string destinationStation, int maximumStops)
        {
            Assert.That(() => m_TripStopCalculator.GetTripsByStops(sourceStation, destinationStation, maximumStops), Throws.TypeOf<InvalidStationException>());
        }
        
        [TestCase("C", "C", 3, 2)]
        [TestCase("A", "C", 4, 3)]
        public void TestGetTripsLessThanXStops(string sourceStation, string destinationStation, int maximumStops, int expectedResults)
        {
            var trips = m_TripStopCalculator.GetTripsByStops(sourceStation, destinationStation, maximumStops);

            Assert.NotNull(trips);
            Assert.AreEqual(expectedResults, trips.Count);
        }

        [TestCase("C", "C", 1)]
        [TestCase("A", "C", 1)]
        public void TestFailGetTripsLessThanXStops(string sourceStation, string destinationStation, int stopLimit)
        {
            var trips = m_TripStopCalculator.GetTripsByStops(sourceStation, destinationStation, stopLimit);

            Assert.Null(trips);
        }
    }
}
