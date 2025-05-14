using System.Net;
using System.Text.Json.Serialization;

namespace BankApi.Domain.Shared;

public class Result<T>
{
    public static implicit operator Result<T>(T value) => new(value, null);

    public bool IsSuccess => Status == HttpStatusCode.Accepted || Status == HttpStatusCode.OK || Status == HttpStatusCode.Created || Status == HttpStatusCode.NoContent;
    public bool IsFailure => !IsSuccess;
    public T Value { get; }
    public string Message { get; init; }
    public HttpStatusCode Status { get; init; }
    [JsonIgnore]
    public Dictionary<string, string[]>? ErrorDetails { get; }

    protected Result(T value, string? message)
    {
        Value = value;
        Message = message ?? "Datos obtenidos exitosamente";
        Status = HttpStatusCode.OK;
    }

    protected Result(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, Dictionary<string, string[]>? errors = null)
    {
        Status = statusCode;
        Value = default!;
        Message = message;
        ErrorDetails = errors;
    }

    public static Result<T> Success(T value, string? message = null) => new(value, message);

    public static Result<T> Success(string message = "Operacion realizada exitosamente") => new(message, HttpStatusCode.OK);

    public static Result<T> Fail(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, Dictionary<string, string[]>? errors = null) =>
        new(message, statusCode, errors);

    public static Result<T> ValidationError(IEnumerable<ValidationError>? errors = null, string message = "Los datos ingresados son inválidos") =>
        new(message, HttpStatusCode.BadRequest, errors?.ToDictionary(x => x.Field, x => x.Messages.ToArray()));

    public static Result<T> ValidationError(ValidationError? error = null, string message = "Los datos ingresados son inválidos")
    {
        if (error is not null)
        {
            var errors = new Dictionary<string, string[]>
            {
                { error.Field, [.. error.Messages] }
            };

            return new(error.Messages.FirstOrDefault() ?? message, HttpStatusCode.BadRequest, errors);
        }

        return new(message, HttpStatusCode.BadRequest);
    }


    public static Result<T> NotFound(string message) => new(message, HttpStatusCode.NotFound);

    public static Result<T> Conflict(string message) => new(message, HttpStatusCode.Conflict);

    public static Result<T> Unauthorized(string message) => new(message, HttpStatusCode.Unauthorized);

    public static Result<T> Forbidden(string message) => new(message, HttpStatusCode.Forbidden);

    public static Result<T> InternalServerError(string message) => new(message, HttpStatusCode.InternalServerError);
}

public static partial class Result
{
    /// <summary>
    /// Objeto para evitar el uso de void
    /// </summary>
    public static readonly Unit Unit = Unit.Value;

    /// <summary>
    /// Encadena un objeto en la estructura de resultados
    /// </summary>
    public static Result<T> Success<T>(this T value) => Result<T>.Success(value);

    /// <summary>
    /// Encadena un Result.Unit en la estructura de resultados
    /// </summary>
    public static Result<Unit> Success() => Result<Unit>.Success(Unit, "Operacion realizada exitosamente");

    /// <summary>
    /// Convierte en asíncrono un resultado
    /// </summary>
    public static Task<Result<T>> Async<T>(this Result<T> r) => Task.FromResult(r);
}
