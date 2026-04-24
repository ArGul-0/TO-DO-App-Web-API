using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Common;

namespace ToDoApp.WebApi.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult(this Result result)
        {
            if (result.IsSuccess)
                return new OkResult();

            return result.Error.Type switch
            {
                ErrorType.Validation => new BadRequestObjectResult(result.Error),
                ErrorType.NotFound => new NotFoundObjectResult(result.Error),
                ErrorType.Unauthorized => new UnauthorizedObjectResult(result.Error),
                ErrorType.Forbidden => new ObjectResult(result.Error) { StatusCode = 403 },
                ErrorType.Conflict => new ConflictObjectResult(result.Error),
                ErrorType.InternalServerError => new ObjectResult(result.Error) { StatusCode = 500 },

                _ => new ObjectResult(result.Error) { StatusCode = 500 }
            };
        }

        public static IActionResult ToActionResult<T>(this ResultT<T> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result.Value);

            return result.ToActionResult();
        }
    }
}
