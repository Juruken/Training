using System;
using System.Collections.Generic;
using TrainTrip.Processors;

namespace TrainTrip.Providers
{
    public class StationDataProvider : IStationDataProvider
    {
        private readonly IDataProvider m_DataProvider;
        private readonly IStationDataProcessor m_StationDataProcessor;
        private readonly Lazy<List<string>> m_ProcessedStationData;

        public StationDataProvider(IDataProvider dataProvider, IStationDataProcessor processor)
        {
            m_DataProvider = dataProvider;
            m_StationDataProcessor = processor;
            m_ProcessedStationData = new Lazy<List<string>>(LoadStationData);
        }

        /// <summary>
        /// Returns a list of strings, each char is the initial of a station name e.g. "A"
        /// </summary>
        /// <returns></returns>
        public List<string> GetData()
        {
            return m_ProcessedStationData.Value;
        }

        private List<string> LoadStationData()
        {
            var data = m_DataProvider.GetData();
            return m_StationDataProcessor.Process(data);
        }
    }
}
