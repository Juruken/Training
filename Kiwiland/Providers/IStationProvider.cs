using Kiwiland.Data;

namespace Kiwiland.Processors
{
    public interface IStationProvider
    {
        Station GetStation(string stationName);
    }
}
