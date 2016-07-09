using System.Collections.Generic;
using Kiwiland.Data;

namespace Kiwiland.Processors
{
    public interface IRouteProvider
    {
        List<Route> GetRoutes(string stationName);
    }
}
