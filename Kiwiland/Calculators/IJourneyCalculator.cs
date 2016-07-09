using System.Collections.Generic;
using Kiwiland.Data;

namespace Kiwiland.Calculators
{
    public interface IJourneyCalculator
    {
        Journey Calculate(string[] routes);
    }
}
