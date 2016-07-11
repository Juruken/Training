using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TrainTrip;
using TrainTrip.Exceptions;
using TrainTrip.Factory;
using TrainTrip.Managers;

namespace KiwilandTests.Component
{
    [TestFixture]
    public class CodingTestComponentTest
    {
        private ITripManager m_TripManager;

        [SetUp]
        public void Setup()
        {
            var m_TestData = "AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7";

            var fileDataProvider = new Mock<IDataProvider>();
            fileDataProvider.Setup(r => r.GetData()).Returns(new List<string> { m_TestData });

            var tripFactory = new TripFactory(',', fileDataProvider.Object);
            m_TripManager = tripFactory.CreateTripManager();
        }

        [Test]
        // GetJourneyDistance
        public void TestInput1()
        {
            // 1.The distance of the route A - B - C. 
            // Expected Output #1: 9
            var result = m_TripManager.GetJourneyDistance(new[] { "A", "B", "C" });
            Assert.NotNull(result);
            Assert.AreEqual(9, result);
        }

        [Test]
        // GetJourneyDistance
        public void TestInput2()
        {
            // 2.The distance of the route A - D.
            // Expected Output #2: 5
            var result = m_TripManager.GetJourneyDistance(new[] { "A", "D" });
            Assert.NotNull(result);
            Assert.AreEqual(5, result);
        }

        [Test]
        // GetJourneyDistance
        public void TestInput3()
        {
            // 3.The distance of the route A - D - C.
            //Expected Output #3: 13
            var result = m_TripManager.GetJourneyDistance(new[] { "A", "D", "C" });
            Assert.NotNull(result);
            Assert.AreEqual(13, result);
        }

        [Test]
        // GetJourneyDistance
        public void TestInput4()
        {
            // 4.The distance of the route A - E - B - C - D.
            //Expected Output #4: 22
            var result = m_TripManager.GetJourneyDistance(new[] { "A", "E", "B", "C", "D" });
            Assert.NotNull(result);
            Assert.AreEqual(22, result);
        }

        [Test]
        // GetJourneyDistance
        public void TestInput5()
        {
            // 5.The distance of the route A - E - D.
            //Expected Output #5: NO SUCH ROUTE
            // Will throw exception.
            Assert.That(() => m_TripManager.GetJourneyDistance(new[] { "A", "E", "D" }), Throws.TypeOf<InvalidTripException>());
        }

        [Test]
        // GetRoutesByMaximumStops
        public void TestInput6()
        {
            // 6.The number of trips starting at C and ending at C with a maximum of 3 stops. In the sample data below, there are two such trips: C - D - C(2 stops).and C - E - B - C(3 stops).
            //Expected Output #6: 2
            var result = m_TripManager.GetRoutesByMaximumStops("C", "C", 3);
            Assert.NotNull(result);
            Assert.AreEqual(2, result);
        }

        [Test]
        // GetExactTripPermutationsCountByStops
        public void TestInput7()
        {
            // 7. The number of trips starting at A and ending at C with exactly 4 stops. 
            // In the sample data below, there are three such trips: A to C(via B, C, D); A to C(via D, C, D); and A to C (via D, E, B).
            //Expected Output #7: 3
            var result = m_TripManager.GetExactTripPermutationsCountByStops("A", "C", 4);
            Assert.NotNull(result);
            Assert.AreEqual(3, result);
        }

        [Test]
        // GetShortestRouteByDistanceWithRecursion
        public void TestInput8()
        {
            // 8. The length of the shortest route (in terms of distance to travel) from A to C.
            //Expected Output #8: 9
            var result = m_TripManager.GetShortestRouteByDistanceWithRecursion("A", "C");
            Assert.NotNull(result);
            Assert.AreEqual(9, result);
        }

        [Test]
        // GetShortestRouteByDistanceWithRecursion
        public void TestInput9()
        {
            // 9.The length of the shortest route(in terms of distance to travel) from B to B.
            //Expected Output #9: 9
            var result = m_TripManager.GetShortestRouteByDistanceWithRecursion("B", "B");
            Assert.NotNull(result);
            Assert.AreEqual(9, result);
        }

        [Test]
        // GetPermutations
        public void TestInput10()
        {
            // 10. The number of different routes from C to C with a distance of less than 30. In the sample data, the trips are: CDC, CEBC, CEBCDC, CDCEBC, CDEBC, CEBCEBC, CEBCEBCEBC.
            //Expected Output #10: 7
            var result = m_TripManager.GetTripPermutationsCountByDistance("C", "C", 30);
            Assert.NotNull(result);
            Assert.AreEqual(7, result);
        }
    }
}
