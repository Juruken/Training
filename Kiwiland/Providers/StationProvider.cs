using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kiwiland.Data;

namespace Kiwiland.Processors
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

        internal Dictionary<string, Station> LoadStations()
        {
            var data = m_DataProvider.GetData();
            
            return data.Select(CreateStation).ToDictionary(s => s.Name, s => s);
        }

        /// <summary>
        /// Expects to get a Station in the form of a string, AB1
        /// Where A is the station name, B is a destination station, and 1 is the distance between them.
        /// </summary>
        /// <param name="stationString"></param>
        /// <returns></returns>
        internal Station CreateStation(string stationString)
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
