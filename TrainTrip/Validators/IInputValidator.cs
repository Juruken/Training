using TrainTrip.Constants;

namespace TrainTrip.Validators
{
    public interface IInputValidator
    {
        bool Validate(string input, InputType inputType);
    }
}
