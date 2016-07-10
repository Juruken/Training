using System.Collections.Generic;
using TrainTrip.DataModel;

namespace TrainTrip.Processors
{
    public interface ITripDirectRouteDistanceCalculator
    {
        Trip GetDirectRouteByLowestDistance(string sourceStation, string destinationStation);
    }
}
