using System;
using System.Collections.Generic;
using System.Linq;
using TrainTrip.Processors;
using TrainTrip.Providers;
using TrainTrip.Validators;
using Moq;
using NUnit.Framework;
using TrainTrip.Exceptions;

namespace TrainTripTests.Processors
{
    [TestFixture]
    public class RouteDataProcessorTests
    {
        private IRouteDataProcessor m_RouteDataProcessor;

        private const string INVALID_ROUTE_STRING = "1AB";

        [SetUp]
        public void SetUp()
        {
            var routeDataValidator = new Mock<IRouteDataValidator>();
            routeDataValidator.Setup(r => r.Validate(It.IsAny<string>())).Returns((string s) => s != INVALID_ROUTE_STRING);

            m_RouteDataProcessor = new RouteDataProcessor(routeDataValidator.Object, ',');
        }

        [Test]
        public void TestValidInput()
        {
            var result = m_RouteDataProcessor.Process(new List<string> { "BA2" + "," + "AB1" });

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("BA2", result.First());
            Assert.AreEqual("AB1", result.Skip(1).First());
        }

        [Test]
        public void TestInvalidData()
        {
            Assert.That(() => m_RouteDataProcessor.Process(new List<string> { INVALID_ROUTE_STRING + "," + "AB1" }), Throws.TypeOf<InvalidRouteException>());
        }
    }
}
