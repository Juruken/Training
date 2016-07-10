using System.Collections;
using System.Collections.Generic;
using TrainTrip;
using TrainTrip.Factory;
using TrainTrip.Managers;
using Moq;
using NUnit.Framework;

namespace TrainTripTests.Component
{
    [TestFixture]
    public class TripManagerTests
    {
        private ITripManager m_TripManager;

        private const string m_TestData = "AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7";

        [SetUp]
        public void Setup()
        {
            var fileDataProvider = new Mock<IDataProvider>();
            fileDataProvider.Setup(r => r.GetData()).Returns(new List<string> { m_TestData });

            var tripFactory = new TripFactory(',', fileDataProvider.Object);
            m_TripManager = tripFactory.CreateTripManager();
        }
        
        [Test]
        public void GetFastestTripByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly)
        {
            var result = m_TripManager.GetFastestTripByDistance(sourceStation, destinationStation, maximumDistance, directRouteOnly);

            Assert.NotNull(result);
        }

        [Test]
        public void GetTripsByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly)
        {
            var result = m_TripManager.GetTripsByDistance(sourceStation, destinationStation, maximumDistance, directRouteOnly);

            Assert.NotNull(result);
        }

        [TestCase("C", "C", 30, 7)]
        public void GetPermutations(string sourceStation, string destinationStation, int maximumDistance, int expectedResult)
        {
            var result = m_TripManager.GetPermutations(sourceStation, destinationStation, maximumDistance);

            Assert.NotNull(result);
            Assert.AreEqual(expectedResult, result.Count);
        }

        [Test]
        public void GetJourney(string[] stations, int maximumDistance, bool directRouteOnly)
        {
            var result = m_TripManager.GetJourney(stations, maximumDistance, directRouteOnly);

            Assert.NotNull(result);
        }
        
        public void GetFastestTripByStops(string sourceStation, string destinationStation, int expectedResult)
        {
            var result = m_TripManager.GetFastestTripByStops(sourceStation, destinationStation);

            Assert.NotNull(result);
            Assert.AreEqual(result.TotalStops, expectedResult);
        }

        
        public void GetTripsByStops(string sourceStation, string destinationStation, int maximumStops)
        {
            var result = m_TripManager.GetTripsByStops(sourceStation, destinationStation, maximumStops);

            Assert.NotNull(result);
        }

        [Test, TestCaseSource(typeof(TestDataProvider), "GetJourneyLengthByDistanceCases")]
        public void GetJourneyLengthByDistance(string[] stations, int maximumDistance, bool directRouteOnly, int expectedResult)
        {
            var result = m_TripManager.GetJourneyLengthByDistance(stations, maximumDistance, directRouteOnly);

            Assert.NotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void TestFailToGetJourney()
        {
            var result = m_TripManager.GetJourneyLengthByDistance(new[] { "A", "E", "D" }, 1000, true);

            Assert.Null(result);
        }

        [Test]
        public void GetJourneyLengthByDistance()
        {
            var result = m_TripManager.GetJourneyLengthByDistance(new[] { "A", "B", "C" }, 1000, true);

            Assert.NotNull(result);
            Assert.AreEqual(9, result);
        }
    }
}

public class TestDataProvider
{
    public static IEnumerable GetJourneyLengthByDistanceCases
    {
        get
        {
            yield return new TestCaseData(new [] { "A", "B", "C" }, 9).SetDescription("The distance of the route A-B-C should be 9.");
            yield return new TestCaseData(new [] { "A", "D" }, 5).SetDescription("The distance of the route A-D should be 5.");
            yield return new TestCaseData(new [] { "A", "D", "C" }, 13).SetDescription("The distance of the route A - D - C should be 13.");
            yield return new TestCaseData(new [] { "A", "E", "B", "C", "D" }, 22).SetDescription("The distance of the route A - E - B - C - D should be 22.");
        }
    }
}