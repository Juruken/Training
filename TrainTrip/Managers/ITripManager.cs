using System.Collections.Generic;
using TrainTrip.DataModel;

namespace TrainTrip.Managers
{
    public interface ITripManager
    {
        List<Trip> GetPermutations(string sourceStation, string destinationStation, int maximumDistance);

        List<Trip> GetRoutesByMaximumStops(string sourceStation, string destinationStation, int maximumStops);
        int GetCountOfRoutesForStationsByStops(string sourceStation, string destinationStation, int maximumStops);
        // TODO: Add Tests.
        int GetCountOfRoutesByExactStops(string sourceStation, string destinationStation, int exactStops);

        Trip GetShortestRouteByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly);
        List<Trip> GetRoutesByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly);

        Journey GetJourney(string[] stations, int maximumDistance, bool directRouteOnly);
    }
}
