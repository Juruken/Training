using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiwiland.Data
{
    public class Journey
    {
        public List<Trip> Trips { get; set; }
        public int Distance { get; set; }
    }
}
