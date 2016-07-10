using System.Collections.Generic;
using TrainTrip.DataModel;

namespace TrainTrip.Managers
{
    public interface ITripManager
    {
        List<Trip> GetPermutations(string sourceStation, string destinationStation, int maximumDistance);

        List<Trip> GetTripsByMaximumStops(string sourceStation, string destinationStation, int maximumStops);
        int GetCountOfTripsForStationsByStops(string sourceStation, string destinationStation, int maximumStops);
        // TODO: Add Tests.
        int GetCountOfTripsByExactStops(string sourceStation, string destinationStation, int exactStops);

        Trip GetShortestRouteByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly);
        List<Trip> GetTripsByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly);

        Journey GetJourney(string[] stations, int maximumDistance, bool directRouteOnly);
    }
}
