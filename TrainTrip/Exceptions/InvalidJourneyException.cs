namespace TrainTrip.Exceptions
{
    public class InvalidJourneyException : TrainTripException
    {
        public InvalidJourneyException (string journey) : base(journey + " is an invalid Journey.")
        {
            
        }
    }
}
