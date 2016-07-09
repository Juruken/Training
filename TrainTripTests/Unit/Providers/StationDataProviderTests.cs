using System.Collections.Generic;
using System.Linq;
using TrainTrip.Processors;
using TrainTrip.Providers;
using Moq;
using NUnit.Framework;
using TrainTrip;

namespace TrainTripTests.Providers
{
    [TestFixture]
    public class StationDataProviderTests
    {
        private IStationDataProvider m_StationDataProvider;

        [SetUp]
        public void SetUp()
        {
            var processedFileData = new List<string>
            {
                "AB1",
                "BD2",
                "CD1",
                "BD3"
            };

            var processor = new Mock<IStationDataProcessor>();
            processor.Setup(r => r.Process(It.IsAny<List<string>>())).Returns(processedFileData);

            var dataProvider = new Mock<IDataProvider>();
            dataProvider.Setup(r => r.GetData()).Returns(new List<string>());
            
            m_StationDataProvider = new StationDataProvider(dataProvider.Object, processor.Object);
        }

        [Test]
        public void TestGetData()
        {
            var data = m_StationDataProvider.GetData();

            Assert.IsNotNull(data);
            Assert.AreEqual(4, data.Count);
            Assert.AreEqual("AB1", data.First());
        }
    }
}
