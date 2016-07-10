using TrainTrip.DataModel;

namespace TrainTrip.Processors
{
    public interface IStationProvider
    {
        Station GetStation(string stationName);
    }
}
