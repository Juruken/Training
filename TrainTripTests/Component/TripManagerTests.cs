using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TrainTrip;
using TrainTrip.Factory;
using TrainTrip.Managers;
using Moq;
using NUnit.Framework;
using TrainTrip.Exceptions;

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

        [TestCase("A","C", 4, 3)]
        public void TestGetCountOfRoutesByExactStops(string sourceStation, string destinationStation, int exactStops, int expectedResults)
        {
            var countOfMatchingTrips = m_TripManager.GetExactTripPermutationsCountByStops(sourceStation, destinationStation, exactStops);

            Assert.NotNull(countOfMatchingTrips);
            Assert.AreEqual(expectedResults, countOfMatchingTrips);
        }

        [TestCase("C", "C", 11, "CEBC")]
        public void GetFastestTripByDistance(string sourceStation, string destinationStation, int expectedDistance, string expectedName)
        {
            var trip = m_TripManager.GetDirectRouteByLowestDistance(sourceStation, destinationStation);

            Assert.NotNull(trip);
            Assert.AreEqual(expectedDistance, trip.TotalDistance);
            Assert.AreEqual(expectedName, trip.TripName);
        }
        
        [TestCase("C", "C", 30, 7)]
        public void GetPermutations(string sourceStation, string destinationStation, int maximumDistance, int expectedResult)
        {
            var count = m_TripManager.GetTripPermutationsCountByDistance(sourceStation, destinationStation, maximumDistance);

            Assert.NotNull(count);
            Assert.AreEqual(expectedResult, count);
        }
        
        [TestCase("C", "F", 5)]
        public void TestInvalidRouteThrowsException(string sourceStation, string destinationStation, int maximumStops)
        {
            Assert.That(() => m_TripManager.GetRoutesByMaximumStops(sourceStation, destinationStation, maximumStops), Throws.TypeOf<InvalidStationException>());
        }

        [TestCase("C", "C", 3, 2)]
        [TestCase("A", "C", 4, 6)]
        public void TestGetTripsLessThanXStops(string sourceStation, string destinationStation, int maximumStops, int expectedResults)
        {
            var trips = m_TripManager.GetTripPermutationsCountByStops(sourceStation, destinationStation, maximumStops);

            Assert.AreEqual(expectedResults, trips);
        }

        [TestCase("C", "C", 1)]
        [TestCase("A", "C", 1)]
        public void TestZeroTripsByMaxmimumStops(string sourceStation, string destinationStation, int stopLimit)
        {
            var count = m_TripManager.GetRoutesByMaximumStops(sourceStation, destinationStation, stopLimit);

            Assert.AreEqual(0, count);
        }

        [Test, TestCaseSource(typeof(TestDataProvider), "GetJourneyDirectRouteCases")]
        public void TestGetJourneyDirectRouteOnly(string[] stations, int maximumDistance, bool directRouteOnly, int expectedResult)
        {
            var journey = m_TripManager.GetJourney(stations);

            Assert.NotNull(journey);
            Assert.AreEqual(expectedResult, journey.Distance);
        }

        [Test]
        public void TestInvalidDirectJourneyThrowsException()
        {
            Assert.That(() => m_TripManager.GetJourney(new[] { "A", "E", "D" }), Throws.TypeOf<InvalidTripException>());
        }
    }
}

public class TestDataProvider
{
    public static IEnumerable GetJourneyDirectRouteCases
    {
        get
        {
            yield return new TestCaseData(new [] { "A", "B", "C" }, 1000, true, 9).SetDescription("The distance of the route A-B-C should be 9.");
            yield return new TestCaseData(new [] { "A", "D" }, 1000, true, 5).SetDescription("The distance of the route A-D should be 5.");
            yield return new TestCaseData(new [] { "A", "D", "C" }, 1000, true, 13).SetDescription("The distance of the route A - D - C should be 13.");
            yield return new TestCaseData(new [] { "A", "E", "B", "C", "D" }, 1000, true, 22).SetDescription("The distance of the route A - E - B - C - D should be 22.");
        }
    }
}