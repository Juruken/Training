using System.Collections.Generic;
using TrainTrip.DataModel;

namespace TrainTrip.Managers
{
    public interface ITripManager
    {
        int? GetShortestRouteByDistanceWithRecursion(string sourceStation, string destinationStation);

        int? GetJourneyDistance(string[] stations);
        
        int GetTripPermutationsCountByDistance(string sourceStation, string destinationStation, int maximumDistance);

        int GetRoutesByMaximumStops(string sourceStation, string destinationStation, int maximumStops);

        int GetTripPermutationsCountByStops(string sourceStation, string destinationStation, int maximumStops);

        int GetExactTripPermutationsCountByStops(string sourceStation, string destinationStation, int exactStops);
    }
}
