namespace BankApi.Domain.Shared;

/// <summary>
/// Represents a unit type, which represents the absence of a specific value.
/// </summary>
public sealed class Unit
{
    public static readonly Unit Value = new();

    private Unit() { }
}