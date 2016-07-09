using System.Collections.Generic;

namespace TrainTrip.Processors
{
    public interface IStationDataProcessor
    {
        List<string> Process(List<string> rawStationData);
    }
}
