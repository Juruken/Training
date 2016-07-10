using TrainTrip.DataModel;

namespace TrainTrip.Managers
{
    public interface ITripManager
    {
        Journey GetJourney(string[] stations, int maximumDistance, bool directRouteOnly);
        Trip GetShortestRouteByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly);
        int GetTripPermutationsCountByDistance(string sourceStation, string destinationStation, int maximumDistance);
        int GetRoutesByMaximumStops(string sourceStation, string destinationStation, int maximumStops);
        int GetTripPermutationsCountByStops(string sourceStation, string destinationStation, int maximumStops);
        int GetExactTripPermutationsCountByStops(string sourceStation, string destinationStation, int exactStops);
        int GetRoutesByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly);
    }
}
