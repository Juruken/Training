using System.Collections.Generic;
using Kiwiland.Calculators;
using Kiwiland.Data;
using NUnit.Framework;

namespace KiwilandTests.Calculators
{
    [TestFixture]
    public class JourneyCalculatorTests : BaseCalculatorTests
    {
        private IJourneyCalculator m_JourneyCalculator;

        [SetUp]
        public void Setup()
        {
            SetupTests();
            // TODO: Determine if we should inject TripCalculator
            m_JourneyCalculator = new JourneyCalculator(m_StationProvider.Object);
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
    }
}
