using System;

namespace Kiwiland.Exceptions
{
    public class InvalidStationFormat : Exception
    {
        public InvalidStationFormat(string station) : base (station + " is an invalid Station format.")
        {
            
        }
    }
}
