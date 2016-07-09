using System.Collections.Generic;
using Kiwiland.Data;

namespace Kiwiland.Managers
{
    public interface ITripManager
    {
        Journey GetJourney(string[] stations);
        int GetJourneyLengthByDistance(string[] stations);

        List<Trip> GetPermutations(string sourceStation, string destinationStation, int maximumDistance);

        Trip GetFastestTripByStops(string sourceStation, string destinationStation);
        List<Trip> GetTripsByStops(string sourceStation, string destinationStation, int maximumStops);

        Trip GetFastestTripByDistance(string sourceStation, string destinationStation, int maximumDistance);
        List<Trip> GetTripsByDistance(string sourceStation, string destinationStation, int maximumDistance);
    }
}
