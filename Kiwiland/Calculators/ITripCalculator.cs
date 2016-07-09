using System.Collections.Generic;
using Kiwiland.Data;

namespace Kiwiland.Processors
{
    public interface ITripCalculator
    {
        Trip GetShortestTrip(string sourceStation, string destinationStation);
        List<Trip> GetTrips(string sourceStation, string destinationStation, int maximumDistance);
    }
}
