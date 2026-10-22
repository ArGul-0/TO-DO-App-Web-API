using ToDoApp.Application.Common;

namespace ToDoApp.WebApi.Extensions;

public static class OpenApiResultExtensions
{
    extension(RouteHandlerBuilder builder)
    {
        public RouteHandlerBuilder ProducesResultErrors(
        params ErrorType[] errorTypes)
        {
            foreach (var errorType in errorTypes.Distinct())
            {
                builder = errorType switch
                {
                    ErrorType.Validation =>
                        builder.Produces<Error>(
                            StatusCodes.Status400BadRequest),

                    ErrorType.NotFound =>
                        builder.Produces<Error>(
                            StatusCodes.Status404NotFound),

                    ErrorType.Unauthorized =>
                        builder.Produces(
                            StatusCodes.Status401Unauthorized),

                    ErrorType.Forbidden =>
                        builder.Produces<Error>(
                            StatusCodes.Status403Forbidden),

                    ErrorType.Conflict =>
                        builder.Produces<Error>(
                            StatusCodes.Status409Conflict),

                    ErrorType.InternalServerError =>
                        builder.ProducesProblem(
                            StatusCodes.Status500InternalServerError),

                    _ => builder
                };
            }

            return builder;
        }
    }
}
