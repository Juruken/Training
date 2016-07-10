using TrainTrip.Calculators;
using NUnit.Framework;
using TrainTrip.Exceptions;
using TrainTripTests.Calculators;

namespace TrainTripTests.Unit.Calculators
{
    [TestFixture]
    public class TripStopCalculatorTests : BaseCalculatorTests
    {
        private ITripPermutationsCalculator m_TripStopCalculator;

        [SetUp]
        public void Setup()
        {
            SetupTests();
            
            m_TripStopCalculator = new TripStopPermutationsCalculator(m_StationProvider.Object);
        }

        [Test]
        public void TestChangingMaximum()
        {
            var firstResult = m_TripStopCalculator.GetPermutations("C", "C", 6);

            Assert.NotNull(firstResult);
            Assert.AreEqual(10, firstResult.Count);

            var secondResult = m_TripStopCalculator.GetPermutations("C", "C", 3);

            Assert.NotNull(secondResult);

            Assert.AreEqual(2, secondResult.Count);

            firstResult = m_TripStopCalculator.GetPermutations("C", "C", 6);

            Assert.AreEqual(10, firstResult.Count);
        }

        [TestCase("C", "F", 5)]
        public void TestInvalidRouteThrowsException(string sourceStation, string destinationStation, int maximumStops)
        {
            Assert.That(() => m_TripStopCalculator.GetPermutations(sourceStation, destinationStation, maximumStops), Throws.TypeOf<InvalidStationException>());
        }
        
        [TestCase("C", "C", 3, 2)]
        [TestCase("A", "C", 4, 6)]
        public void TestGetTripsLessThanXStops(string sourceStation, string destinationStation, int maximumStops, int expectedResults)
        {
            var trips = m_TripStopCalculator.GetPermutations(sourceStation, destinationStation, maximumStops);

            Assert.NotNull(trips);
            Assert.AreEqual(expectedResults, trips.Count);
        }

        [TestCase("C", "C", 1)]
        [TestCase("A", "C", 1)]
        public void TestFailGetTripsLessThanXStops(string sourceStation, string destinationStation, int stopLimit)
        {
            var trips = m_TripStopCalculator.GetPermutations(sourceStation, destinationStation, stopLimit);

            Assert.Null(trips);
        }
    }
}
