using System.Collections.Generic;

namespace TrainTrip.Data
{
    public class Station
    {
        public string Name { get; set; }
        public Dictionary<string, Route> Routes { get; set; }
    }
}
