using System;
using System.Collections.Generic;
using Kiwiland.Data;
using Kiwiland.Processors;

namespace Kiwiland.Providers
{
    public class RouteProvider : IRouteProvider
    {
        private readonly IRouteDataProvider m_RouteDataProvider;
        private readonly Lazy<Dictionary<string, List<Route>>> m_RoutesByStationName;

        public RouteProvider(IRouteDataProvider dataProvider)
        {
            m_RouteDataProvider = dataProvider;
            m_RoutesByStationName = new Lazy<Dictionary<string, List<Route>>>(LoadRoutes);
        }
        
        public List<Route> GetRoutes(string stationName)
        {
            throw new System.NotImplementedException();
        }

        internal Dictionary<string, List<Route>> LoadRoutes()
        {
            var routesByStationName = new Dictionary<string, List<Route>>();

            var data = m_RouteDataProvider.GetData();

            // TODO: Create the Routes

            return routesByStationName;
        }
    }
}
