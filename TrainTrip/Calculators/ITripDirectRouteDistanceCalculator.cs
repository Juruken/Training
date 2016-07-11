using System.Collections.Generic;
using TrainTrip.DataModel;

namespace TrainTrip.Processors
{
    public interface ITripDirectRouteDistanceCalculator
    {
        Trip GetDirectRouteByLowestDistanceWithRecursion(string sourceStation, string destinationStation);
        Trip GetDirectRouteByLowestDistanceWithoutRecursion(string sourceStation, string destinationStation);
    }
}
