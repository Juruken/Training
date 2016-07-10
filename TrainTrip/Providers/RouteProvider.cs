using System;
using System.Collections.Generic;
using TrainTrip.DataModel;
using TrainTrip.Processors;

namespace TrainTrip.Providers
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
            return m_RoutesByStationName.Value.ContainsKey(stationName) ? m_RoutesByStationName.Value[stationName] : null;
        }

        private Dictionary<string, List<Route>> LoadRoutes()
        {
            var routesByStationName = new Dictionary<string, List<Route>>();

            var data = m_RouteDataProvider.GetData();

            foreach (var routeString in data)
            {
                var route = new Route()
                {
                    SourceStation = routeString.Substring(0, 1),
                    DestinationStation = routeString.Substring(1, 1),
                    Distance = int.Parse(routeString.Substring(2, 1))
                };

                if (routesByStationName.ContainsKey(route.SourceStation))
                {
                    routesByStationName[route.SourceStation].Add(route);
                }
                else
                {
                    routesByStationName.Add(route.SourceStation, new List<Route>() { route });
                }
            }

            return routesByStationName;
        }
    }
}
