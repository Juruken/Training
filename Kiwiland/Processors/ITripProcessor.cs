using System.Collections.Generic;
using Kiwiland.Data;

namespace Kiwiland.Processors
{
    public interface ITripProcessor
    {
        List<Trip> Process(string sourceStation, string destinationStation, int maximumDistance);
    }
}
