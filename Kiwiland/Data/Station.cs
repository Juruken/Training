using System.Collections.Generic;

namespace Kiwiland.Data
{
    public class Station
    {
        public string Name { get; set; }
        public Dictionary<string, Route> Routes { get; set; }
    }
}
