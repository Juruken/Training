namespace TrainTrip.DataModel
{
    public class Trip
    {
        public string TripName { get; set; }
        public int TotalDistance { get; set; }

        public int TotalStops
        {
            get
            {
                return TripName != null ? TripName.Length : 0;
            }
        }
        
        public Trip Clone()
        {
            return new Trip
            {
                TotalDistance = TotalDistance,
                TripName = TripName
            };
        }
    }
}
