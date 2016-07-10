namespace TrainTrip.Exceptions
{
    public class InvalidTripException : TrainTripException
    {
        public InvalidTripException(string trip) : base(trip + " is an invalid trip.")
        {
            
        }
    }
}
