using Microsoft.AspNetCore.Mvc;

namespace Etymon.Result.Extensions;

/// <summary>
/// Provides extension methods for converting <see cref="Result{T}"/> to HTTP responses.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts a <see cref="Result{T}"/> to an appropriate <see cref="ActionResult{T}"/>
    /// based on its result code.
    /// </summary>
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.Error?.Code switch
        {
            nameof(ResultCode.NotFound) => new NotFoundObjectResult(result),
            nameof(ResultCode.ValidationError) => new BadRequestObjectResult(result),
            nameof(ResultCode.Unauthorized) => new ForbidResult(),
            nameof(ResultCode.Conflict) => new ConflictObjectResult(result),
            nameof(ResultCode.InternalError) => new ObjectResult(result) { StatusCode = 500 },
            _ => new OkObjectResult(result)
        };
    }
}