using System.Collections.Generic;
using Kiwiland.Data;

namespace Kiwiland.Processors
{
    public interface ITripDistanceCalculator
    {
        Trip GetFastestTripByDistance(string sourceStation, string destinationStation);
        List<Trip> GetTripsByDistance(string sourceStation, string destinationStation, int maximumDistance);
    }
}
