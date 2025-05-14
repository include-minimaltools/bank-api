namespace BankApi.Api.Options;

public class CreatedResultOptions(string actionName, object routeValues)
{
    public string ActionName { get; set; } = actionName;
    public object RouteValues { get; set; } = routeValues;
}
