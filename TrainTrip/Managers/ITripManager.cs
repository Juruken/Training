using System.Collections.Generic;
using TrainTrip.DataModel;

namespace TrainTrip.Managers
{
    public interface ITripManager
    {
        List<Trip> GetPermutations(string sourceStation, string destinationStation, int maximumDistance);

        List<Trip> GetTripsByStops(string sourceStation, string destinationStation, int maximumStops);

        Trip GetFastestTripByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly);
        List<Trip> GetTripsByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly);

        Journey GetJourney(string[] stations, int maximumDistance, bool directRouteOnly);
    }
}
