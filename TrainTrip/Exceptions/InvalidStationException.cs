using System;

namespace TrainTrip.Exceptions
{
    public class InvalidStationException : Exception
    {
        public InvalidStationException(string stationName) : base(stationName + " is an invalid Station name.")
        {
            
        }
    }
}
