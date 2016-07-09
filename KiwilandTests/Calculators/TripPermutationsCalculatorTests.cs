using System.Collections.Generic;
using Kiwiland.Calculators;
using Kiwiland.Data;
using Kiwiland.Processors;
using Moq;
using NUnit.Framework;

namespace KiwilandTests.Calculators
{
    [TestFixture]
    public class TripPermutationsCalculatorTests : BaseCalculatorTests
    {
        private ITripPermutationsCalculator m_TripPermutationsCalculator;

        [SetUp]
        public void SetUp()
        {
            m_TripPermutationsCalculator = new TripPermutationsCalculator(m_StationProvider.Object);
        }

        // Total Permutations for permutations calculator
        // CDC, CEBC, CEBCDC, CDCEBC, CDEBC, CEBCEBC, CEBCEBCEBC
        [Test]
        public void TestPermutations()
        {
            var trips = m_TripPermutationsCalculator.GetPermutations("C", "C", 30);

            Assert.NotNull(trips);
            Assert.AreEqual(7, trips.Count);

            // TODO: Check for each permutation? boring...
        }

        private List<Trip> CalculatedTrips()
        {
            return new List<Trip>()
            {
                new Trip()
                {
                    TotalDistance = 11,
                    TripName = "BEBC"
                },
                new Trip()
                {
                    TotalDistance = 16,
                    TripName = "CDC"
                }
            };
        }
    }
}
