namespace BankApi.Domain.Shared;

public class ValidationError
{
    public string Field { get; init; }
    public IEnumerable<string> Messages { get; init; }

    public ValidationError(string field, string message)
    {
        Field = field;
        Messages = [message];
    }

    public ValidationError(string field, IEnumerable<string> message)
    {
        Field = field;
        Messages = message;
    }
}