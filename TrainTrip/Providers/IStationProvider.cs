using TrainTrip.Data;

namespace TrainTrip.Processors
{
    public interface IStationProvider
    {
        Station GetStation(string stationName);
    }
}
