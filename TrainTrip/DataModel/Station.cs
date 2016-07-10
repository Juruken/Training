using System.Collections.Generic;

namespace TrainTrip.DataModel
{
    public class Station
    {
        public string Name { get; set; }
        public Dictionary<string, Route> Routes { get; set; }
    }
}
