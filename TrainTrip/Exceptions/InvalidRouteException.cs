using System;

namespace TrainTrip.Exceptions
{
    public class InvalidRouteException : Exception
    {
        public InvalidRouteException(string routes) : base(routes + " is not a valid route")
        {
            
        }
    }
}
