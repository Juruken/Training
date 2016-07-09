using System;
using System.Collections.Generic;

namespace TrainTrip.Providers
{
    public class RouteDataProvider : IRouteDataProvider
    {
        private readonly IDataProvider m_DataProvider;
        private readonly IRouteDataProcessor m_RouteDataProcessor;

        private readonly Lazy<List<string>> m_ProcessedRouteData;

        public RouteDataProvider(IDataProvider dataProvider, IRouteDataProcessor processor)
        {
            m_RouteDataProcessor = processor;
            m_DataProvider = dataProvider;

            m_ProcessedRouteData = new Lazy<List<string>>(LoadRouteData);
        }

        /// <summary>
        /// Returns a list of strings, each string is a route e.g. "AB1".
        /// </summary>
        /// <returns></returns>
        public List<string> GetData()
        {
            return m_ProcessedRouteData.Value;
        }

        private List<string> LoadRouteData()
        {
            var data = m_DataProvider.GetData();
            return m_RouteDataProcessor.Process(data);
        }
    }
}
