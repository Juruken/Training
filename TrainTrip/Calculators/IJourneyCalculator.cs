using TrainTrip.Data;

namespace TrainTrip.Calculators
{
    public interface IJourneyCalculator
    {
        Journey GetJourneyByRoutes(string[] routes, int maximumDistance, bool directRouteOnly);
    }
}
