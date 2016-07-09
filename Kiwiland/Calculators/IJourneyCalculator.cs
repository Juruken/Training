using System.Collections.Generic;
using Kiwiland.Data;

namespace Kiwiland.Calculators
{
    public interface IJourneyCalculator
    {
        Journey Calculate(List<Route> routes);
    }
}
