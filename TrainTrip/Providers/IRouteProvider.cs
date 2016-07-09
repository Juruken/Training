using System.Collections.Generic;
using TrainTrip.Data;

namespace TrainTrip.Processors
{
    public interface IRouteProvider
    {
        List<Route> GetRoutes(string stationName);
    }
}
