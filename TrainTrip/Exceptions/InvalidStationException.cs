namespace TrainTrip.Exceptions
{
    public class InvalidStationException : TrainTripException
    {
        public InvalidStationException(string stationName) : base(stationName + " is an invalid Station name.")
        {
            
        }
    }
}
