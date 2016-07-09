using System;

namespace Kiwiland.Exceptions
{
    public class InvalidTripException : Exception
    {
        public InvalidTripException(string trip) : base(trip + " is an invalid trip.")
        {
            
        }
    }
}
