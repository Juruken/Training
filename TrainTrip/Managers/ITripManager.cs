using System.Collections.Generic;
using TrainTrip.Data;

namespace TrainTrip.Managers
{
    public interface ITripManager
    {
        List<Trip> GetPermutations(string sourceStation, string destinationStation, int maximumDistance);

        Trip GetFastestTripByStops(string sourceStation, string destinationStation);
        List<Trip> GetTripsByStops(string sourceStation, string destinationStation, int maximumStops);

        Trip GetFastestTripByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly);
        List<Trip> GetTripsByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly);

        Journey GetJourney(string[] stations, int maximumDistance, bool directRouteOnly);
        int GetJourneyLengthByDistance(string[] stations, int maximumDistance, bool directRouteOnly);
    }
}
