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

            m_StationsByName = new Lazy<Dictionary<string, Station>>();
        }

        private Dictionary<string, Station> LoadStations()
        {
            var stations = new Dictionary<string, Station>();

            var data = m_DataProvider.GetData();

            foreach (var stationString in data)
            {
                /*if ()
                {
                    var station = CreateStationFromRoute(stationString);
                }
                else
                {
                    stations.Add();
                }*/
            }

            return stations;
        }

        public Station GetStation(string stationName)
        {
            return m_StationsByName.Value.ContainsKey(stationName) ? m_StationsByName.Value[stationName] : null;
        }

        /// <summary>
        /// Takes a Station in the format of a string. Expected format: <SourceStationInitial, DestinationStationInitial, Distance> e.g. AB1
        /// </summary>
        /// <param name="stationRoute"></param>
        /// <returns></returns>
        private Station CreateStationFromRoute(string stationRoute)
        {
            if (stationRoute.Length != 3)
            {
                throw new ArgumentException("Invalid station data provided");
            }
            
            return new Station { Name = stationRoute.Substring(0, 1) };
        }
    }
}
