using System;
using System.Collections.Generic;
using System.Linq;
using TrainTrip.Calculators;
using NUnit.Framework;

namespace TrainTripTests.Calculators
{
    [TestFixture]
    public class TripPermutationsCalculatorTests : BaseCalculatorTests
    {
        private ITripPermutationsCalculator m_TripPermutationsCalculator;

        [SetUp]
        public void SetUp()
        {
            SetupTests();
            m_TripPermutationsCalculator = new TripDistancePermutationsCalculator(m_StationProvider.Object);
        }

        [Test]
        public void TestChangingMaxDistance()
        {
            var trips = m_TripPermutationsCalculator.GetPermutations("C", "C", 55);

            Assert.NotNull(trips);
            Assert.AreEqual(57, trips.Count);

            trips = m_TripPermutationsCalculator.GetPermutations("C", "C", 50);

            Assert.NotNull(trips);
            Assert.AreEqual(39, trips.Count);

            trips = m_TripPermutationsCalculator.GetPermutations("C", "C", 55);

            Assert.NotNull(trips);
            Assert.AreEqual(57, trips.Count);
        }

        // Total Permutations for permutations calculator
        // CDC, CEBC, CEBCDC, CDCEBC, CDEBC, CEBCEBC, CEBCEBCEBC
        [Test]
        public void TestGetPermutations()
        {
            var trips = m_TripPermutationsCalculator.GetPermutations("C", "C", 30);

            Assert.NotNull(trips);
            Assert.AreEqual(7, trips.Count);

            var expectedTripNames = new List<string>
            {
                "CDC",
                "CEBC",
                "CEBCDC",
                "CDCEBC",
                "CDEBC",
                "CEBCEBC",
                "CEBCEBCEBC"
            };

            foreach (var expectedTripName in expectedTripNames)
            {
                if (trips.All(t => t.TripName != expectedTripName))
                    Assert.Fail(String.Format("Failed to find trip {0}", expectedTripName));
            }
        }
    }
}
