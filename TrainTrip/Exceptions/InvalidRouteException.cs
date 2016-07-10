namespace TrainTrip.Exceptions
{
    public class InvalidRouteException : TrainTripException
    {
        public InvalidRouteException(string routes) : base(routes + " is not a valid route")
        {
            
        }
    }
}
