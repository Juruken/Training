using TrainTrip.Data;

namespace TrainTrip.Calculators
{
    public interface IJourneyCalculator
    {
        Journey Calculate(string[] routes);
    }
}
