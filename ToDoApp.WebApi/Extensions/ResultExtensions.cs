using Microsoft.AspNetCore.Http;
using ToDoApp.Application.Common;

namespace ToDoApp.WebApi.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToHttpResult(this Result result)
        {
            if (result.IsSuccess)
                return Results.Ok();

            return result.Error.Type switch
            {
                ErrorType.Validation => Results.BadRequest(result.Error),
                ErrorType.NotFound => Results.NotFound(result.Error),
                ErrorType.Unauthorized => Results.Unauthorized(),
                ErrorType.Forbidden => Results.Json(result.Error, statusCode: 403),
                ErrorType.Conflict => Results.Conflict(result.Error),
                ErrorType.InternalServerError => Results.Problem(
                                                    detail: result.Error.Description,
                                                    statusCode: 500),

                _ => Results.Problem(detail: result.Error.Description, statusCode: 500)
            };
        }

        public static IResult ToHttpResult<T>(this ResultT<T> result)
        {
            if (result.IsSuccess)
                return Results.Ok(result.Value);

            return ((Result)result).ToHttpResult();
        }
    }
}