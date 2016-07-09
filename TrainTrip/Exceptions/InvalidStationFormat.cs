using System;

namespace TrainTrip.Exceptions
{
    public class InvalidStationFormat : Exception
    {
        public InvalidStationFormat(string station) : base (station + " is an invalid Station format.")
        {
            
        }
    }
}
