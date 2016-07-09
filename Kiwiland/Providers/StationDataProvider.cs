using System;
using System.Collections.Generic;
using Kiwiland.Processors;

namespace Kiwiland.Providers
{
    public class StationDataProvider : IStationDataProvider
    {
        private readonly string m_RawStationData;
        private readonly IStationDataProcessor m_StationDataProcessor;

        private readonly Lazy<List<string>> m_ProcessedStationData;

        public StationDataProvider(IStationDataProcessor processor, string rawStationData)
        {
            m_RawStationData = rawStationData;

            m_ProcessedStationData = new Lazy<List<string>>(LoadStationData);
        }

        public List<string> GetData()
        {
            return m_ProcessedStationData.Value;
        }

        internal List<string> LoadStationData()
        {
            return m_StationDataProcessor.Process(m_RawStationData);
        }
    }
}
