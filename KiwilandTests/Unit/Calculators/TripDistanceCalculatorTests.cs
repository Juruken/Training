﻿using System;
using System.Linq;
using Kiwiland.Processors;
using KiwilandTests.Calculators;
using NUnit.Framework;

namespace KiwilandTests.Processors
{
    [TestFixture]
    public class TripDistanceCalculatorTests : BaseCalculatorTests
    {
        private ITripDistanceCalculator m_TripDistanceCalculator;

        [SetUp]
        public void Setup()
        {
            SetupTests();
            m_TripDistanceCalculator = new TripDistanceDistanceCalculator(m_StationProvider.Object);
        }
        
        [Test]
        public void TestGetShortestTrip()
        {
            var trip = m_TripDistanceCalculator.GetFastestTripByDistance("C", "C");

            Assert.NotNull(trip);
            Assert.AreEqual(11, trip.TotalDistance);
            Assert.AreEqual("CEBC", trip.TripName);
        }

        [Test]
        public void TestFailToGetInvalidTrip()
        {           
            Assert.That(() => m_TripDistanceCalculator.GetFastestTripByDistance("C", "F"), Throws.TypeOf<ArgumentException>());
        }

        [TestCase("C", "C", 30, 2, "CEBC", 11)]
        public void TestGenerateTrips(string sourceStation, string destinationStation, int maximumDistance, int expectedTripCount, string expectedShortestTripName, int expectedDistance)
        {
            var trips = m_TripDistanceCalculator.GetTripsByDistance(sourceStation, destinationStation, maximumDistance);
            
            Assert.NotNull(trips);
            Assert.AreEqual(expectedTripCount, trips.Count);

            var shortestTrip = trips.OrderBy(t => t.TotalDistance).First();
            Assert.AreEqual(expectedShortestTripName, shortestTrip.TripName);
            Assert.AreEqual(expectedDistance, shortestTrip.TotalDistance);
        }
    }
}