using System;

namespace Kiwiland.Exceptions
{
    public class InvalidStationException : Exception
    {
        public InvalidStationException(string stationName) : base(stationName + " is an invalid Station name.")
        {
            
        }
    }
}
