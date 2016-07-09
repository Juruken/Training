using System.Collections.Generic;
using TrainTrip.Data;

namespace TrainTrip.Processors
{
    public interface ITripDistanceCalculator
    {
        Trip GetFastestTripByDistance(string sourceStation, string destinationStation);
        List<Trip> GetTripsByDistance(string sourceStation, string destinationStation, int maximumDistance);
    }
}
