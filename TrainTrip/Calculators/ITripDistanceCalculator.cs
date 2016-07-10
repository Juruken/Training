using System.Collections.Generic;
using TrainTrip.DataModel;

namespace TrainTrip.Processors
{
    public interface ITripDistanceCalculator
    {
        Trip GetFastestTripByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRoutesOnly);
        List<Trip> GetTripsByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRoutesOnly);
    }
}
