using System.Collections.Generic;
using System.Linq;
using Kiwiland.Processors;
using Kiwiland.Providers;
using Moq;
using NUnit.Framework;

namespace KiwilandTests.Providers
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

            m_StationDataProvider = new StationDataProvider(processor.Object, new List<string>());
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
