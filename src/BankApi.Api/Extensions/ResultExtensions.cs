using System.Net;
using BankApi.Api.Options;
using BankApi.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller, CreatedResultOptions? createdOptions = null)
    {
        if (result.IsSuccess)
            return result.Status switch
            {
                HttpStatusCode.NoContent => controller.NoContent(),
                HttpStatusCode.Accepted => controller.Accepted(result),
                HttpStatusCode.OK => controller.Ok(result),
                HttpStatusCode.Created => controller.CreatedAtAction(createdOptions?.ActionName, createdOptions?.RouteValues, result),
                _ => controller.StatusCode((int)result.Status, result)
            };

        var (status, title, type) = result.Status switch
        {
            HttpStatusCode.BadRequest => (400, "Error de validación", "https://httpstatuses.com/400"),
            HttpStatusCode.NotFound => (404, "No encontrado", "https://httpstatuses.com/404"),
            HttpStatusCode.Conflict => (409, "Conflicto", "https://httpstatuses.com/409"),
            HttpStatusCode.Unauthorized => (401, "No autorizado", "https://httpstatuses.com/401"),
            HttpStatusCode.Forbidden => (403, "Prohibido", "https://httpstatuses.com/403"),
            _ => (500, "Error crítico", "https://httpstatuses.com/500")
        };

        var problemDetails = new ProblemDetails
        {
            Title = title,
            Type = type,
            Status = status,
            Detail = result.Message,
            Instance = controller.HttpContext?.Request?.Path,
        };

        problemDetails.Extensions.Add("errors", result.ErrorDetails);

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }
}
