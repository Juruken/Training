using System;
using System.Collections.Generic;
using System.Linq;
using Kiwiland.Processors;
using Kiwiland.Validators;
using Moq;
using NUnit.Framework;

namespace KiwilandTests.Processors
{
    [TestFixture]
    public class StationDataProcessorTests
    {
        private IStationDataProcessor m_StationDataProcessor;

        private const string INVALID_STATION_STRING = "123";

        [SetUp]
        public void SetUp()
        {
            var stationDataValidator = new Mock<IStationDataValidator>();
            stationDataValidator.Setup(r => r.Validate(It.IsAny<string>())).Returns((string s) => s != INVALID_STATION_STRING);

            m_StationDataProcessor = new StationDataProcessor(stationDataValidator.Object, ',');
        }

        [Test]
        public void TestValidInput()
        {
               var result = m_StationDataProcessor.Process(new List<string>{ "BA2" + "," + "AB1"});

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("BA2", result.First());
            Assert.AreEqual("AB1", result.Skip(1).First());
        }

        [Test]
        public void TestInvalidData()
        {
            Assert.That(() => m_StationDataProcessor.Process(new List<string> { INVALID_STATION_STRING + "," + "AB1" }), Throws.TypeOf<ArgumentException>());
        }
    }
}
