namespace TrainTrip.Data
{
    public class Route
    {
        public string SourceStation { get; set; }
        public string DestinationStation { get; set; }
        public int Distance { get; set; }

        public Trip ConvertToTrip()
        {
            return new Trip()
            {
                TotalDistance = Distance,
                TripName = SourceStation + DestinationStation
            };
        }
    }
}
