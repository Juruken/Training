using System.Collections;
using System.Collections.Generic;
using Kiwiland;
using Kiwiland.Factory;
using Kiwiland.Managers;
using Moq;
using NUnit.Framework;

namespace KiwilandTests.Component
{
    [TestFixture]
    public class TripManagerTests
    {
        private ITripManager m_TripManager;

        private const string m_TestData = "AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7";

        [SetUp]
        public void Setup()
        {
            var fileDataProvider = new Mock<IFileDataProvider>();
            fileDataProvider.Setup(r => r.GetFileData()).Returns(new List<string> { m_TestData });

            var tripFactory = new TripFactory(',', fileDataProvider.Object);
            m_TripManager = tripFactory.CreateTripManager();
        }

        [TestCase]
        public void GetFastestTripByStops(string sourceStation, string destinationStation)
        {
            var result = m_TripManager.GetFastestTripByStops(sourceStation, destinationStation);

            Assert.NotNull(result);
        }

        [Test]
        public void GetTripsByStops(string sourceStation, string destinationStation, int maximumStops)
        {
            var result = m_TripManager.GetTripsByStops(sourceStation, destinationStation, maximumStops);

            Assert.NotNull(result);
        }

        [Test]
        public void GetFastestTripByDistance(string sourceStation, string destinationStation, int maximumDistance)
        {
            var result = m_TripManager.GetFastestTripByDistance(sourceStation, destinationStation, maximumDistance);

            Assert.NotNull(result);
        }

        [Test]
        public void GetTripsByDistance(string sourceStation, string destinationStation, int maximumDistance)
        {
            var result = m_TripManager.GetTripsByDistance(sourceStation, destinationStation, maximumDistance);

            Assert.NotNull(result);
        }

        [Test]
        public void GetPermutations(string sourceStation, string destinationStation, int maximumDistance)
        {
            var result = m_TripManager.GetPermutations(sourceStation, destinationStation, maximumDistance);

            Assert.NotNull(result);
        }

        [Test]
        public void GetJourney(string[] stations)
        {
            var result = m_TripManager.GetJourney(stations);

            Assert.NotNull(result);
        }

        /*[Test, TestCaseSource(typeof(TestDataProvider), "GetJourneyLengthByDistanceCases")]
        public int GetJourneyLengthByDistance(string[] stations)
        {
            var result = m_TripManager.GetJourneyLengthByDistance(stations);

            Assert.NotNull(result);

            return result;
        }*/

        [Test]
        public void GetJourneyLengthByDistance()
        {
            var result = m_TripManager.GetJourneyLengthByDistance(new[] { "A", "B", "C" });

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
            yield return new TestCaseData("A", "B", "C").Returns(9).SetDescription("The distance of the route A-B-C should be 9.");
            yield return new TestCaseData("A", "D").Returns(5).SetDescription("The distance of the route A-D should be 5.");
            yield return new TestCaseData("A", "D", "C").Returns(13).SetDescription("The distance of the route A - D - C should be 13.");
            yield return new TestCaseData("A", "E", "B", "C", "D").Returns(22).SetDescription("The distance of the route A - E - B - C - D should be 22.");
        }
    }
}