using System;
using System.Collections.Generic;

namespace Kiwiland.Providers
{
    public class RouteDataProvider : IRouteDataProvider
    {
        private readonly List<string> m_RawRouteData;
        private readonly IRouteDataProcessor m_RouteDataProcessor;

        private readonly Lazy<List<string>> m_ProcessedRouteData;

        public RouteDataProvider(IRouteDataProcessor processor, List<string> rawRouteData)
        {
            m_RouteDataProcessor = processor;
            m_RawRouteData = rawRouteData;

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
            return m_RouteDataProcessor.Process(m_RawRouteData);
        }
    }
}
