using System.Collections.Generic;
using System.Linq;

namespace Kiwiland.Data
{
    public class Journey
    {
        public List<Trip> Trips { get; set; }
        public int Distance
        {
            get
            {
                if (Trips == null)
                    return 0;

                var count = 0;

                foreach (var trip in Trips)
                {
                    count += trip.TotalDistance;
                }

                return count;
            }
        }
    }
}
