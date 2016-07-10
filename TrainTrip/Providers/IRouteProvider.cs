using System.Collections.Generic;
using TrainTrip.DataModel;

namespace TrainTrip.Processors
{
    public interface IRouteProvider
    {
        List<Route> GetRoutes(string stationName);
    }
}
