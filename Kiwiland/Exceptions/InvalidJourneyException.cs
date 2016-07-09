using System;

namespace Kiwiland.Exceptions
{
    public class InvalidJourneyException : Exception
    {
        public InvalidJourneyException (string journey) : base(journey + " is an invalid Journey.")
        {
            
        }
    }
}
