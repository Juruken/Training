using System;
using System.Collections.Generic;
using System.Linq;
using TrainTrip.DataModel;

namespace TrainTrip.Processors
{
    public class StationProvider : IStationProvider
    {
        private readonly IStationDataProvider m_DataProvider;
        private readonly IRouteProvider m_RouteProvider;

        private Lazy<Dictionary<string, Station>> m_StationsByName;
        
        public StationProvider(IStationDataProvider dataProvider, IRouteProvider routeProvider)
        {
            m_DataProvider = dataProvider;
            m_RouteProvider = routeProvider;

            m_StationsByName = new Lazy<Dictionary<string, Station>>(LoadStations);
        }

        public Station GetStation(string stationName)
        {
            return m_StationsByName.Value.ContainsKey(stationName) ? m_StationsByName.Value[stationName] : null;
        }

        private Dictionary<string, Station> LoadStations()
        {
            var stationsByStationName = new Dictionary<string, Station>();

            var data = m_DataProvider.GetData();

            foreach (var stationString in data)
            {
                var station = CreateStation(stationString);

                if (stationsByStationName.ContainsKey(station.Name))
                {
                    var existingStation = stationsByStationName[station.Name];

                    foreach (var route in station.Routes)
                    {
                        if (!existingStation.Routes.ContainsKey(route.Key))
                            existingStation.Routes.Add(route.Key, route.Value);
                    }
                }
                else
                {
                    stationsByStationName.Add(station.Name, station);
                }
            }
            
            return stationsByStationName;
        }

        /// <summary>
        /// Expects to get a Station in the form of a string, AB1
        /// Where A is the station name, B is a destination station, and 1 is the distance between them.
        /// </summary>
        /// <param name="stationString"></param>
        /// <returns></returns>
        private Station CreateStation(string stationString)
        {
            var stationName = stationString.Substring(0, 1);

            var routes = m_RouteProvider.GetRoutes(stationName);
            var routesByDestinationStationName = routes != null ? routes.ToDictionary(r => r.DestinationStation, r => r) : null;

            return new Station()
            {
                Name = stationName,
                Routes = routesByDestinationStationName
            };
        }
    }
}
