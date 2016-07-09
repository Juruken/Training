using System.Collections.Generic;
using Kiwiland.Data;

namespace Kiwiland.Calculators
{
    public interface ITripPermutationsCalculator
    {
        List<Trip> GetPermutations(string sourceStation, string destinationStation, int maximumDistance);
    }
}
