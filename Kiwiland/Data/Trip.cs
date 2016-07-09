namespace Kiwiland.Data
{
    public class Trip
    {
        public string TripName { get; set; } 
        public int TotalDistance { get; set; }

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
