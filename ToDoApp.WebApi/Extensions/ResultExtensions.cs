using ToDoApp.Application.Common;

namespace ToDoApp.WebApi.Extensions
{
    public static class ResultExtensions
    {
        extension(Result result)
        {
            public IResult ToHttpResult()
            {
                if (result.IsSuccess)
                    return Results.Ok();

                return result.Error.Type switch
                {
                    ErrorType.Validation => Results.BadRequest(result.Error),
                    ErrorType.NotFound => Results.NotFound(result.Error),
                    ErrorType.Unauthorized => Results.Unauthorized(),
                    ErrorType.Forbidden => Results.Json(result.Error, statusCode: StatusCodes.Status403Forbidden),
                    ErrorType.Conflict => Results.Conflict(result.Error),
                    ErrorType.InternalServerError => Results.Problem(
                                                        detail: result.Error.Description,
                                                        statusCode: StatusCodes.Status500InternalServerError),

                    _ => Results.Problem(detail: result.Error.Description, statusCode: StatusCodes.Status500InternalServerError)
                };
            }
        }

        extension<T>(ResultT<T> result)
        {
            public IResult ToHttpResult()
            {
                if (result.IsSuccess)
                    return Results.Ok(result.Value);

                return ((Result)result).ToHttpResult();
            }
        }
    }
}