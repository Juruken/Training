using System;
using System.Collections.Generic;
using Kiwiland.Processors;

namespace Kiwiland.Providers
{
    public class StationDataProvider : IStationDataProvider
    {
        private readonly List<string> m_RawStationData;
        private readonly IStationDataProcessor m_StationDataProcessor;

        private readonly Lazy<List<string>> m_ProcessedStationData;

        public StationDataProvider(IStationDataProcessor processor, List<string> rawStationData)
        {
            m_StationDataProcessor = processor;
            m_RawStationData = rawStationData;

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
            return m_StationDataProcessor.Process(m_RawStationData);
        }
    }
}
