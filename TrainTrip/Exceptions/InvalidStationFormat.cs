namespace TrainTrip.Exceptions
{
    public class InvalidStationFormat : TrainTripException
    {
        public InvalidStationFormat(string station) : base (station + " is an invalid Station format.")
        {
            
        }
    }
}
