using Kiwiland.Calculators;
using KiwilandTests.Calculators;
using NUnit.Framework;

namespace KiwilandTests.Unit.Calculators
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

        [Test]
        public void TestGetTrip()
        {
            var trip = m_TripStopCalculator.GetFastestTipByStops("C", "C");

            Assert.NotNull(trip);
            Assert.AreEqual(2, trip.TotalStops);
            Assert.AreEqual("CDC", trip.TripName);
        }
    }
}
