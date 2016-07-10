using TrainTrip.DataModel;

namespace TrainTrip.Calculators
{
    public interface IJourneyCalculator
    {
        Journey GetJourneyByRoutes(string[] routes);
    }
}
