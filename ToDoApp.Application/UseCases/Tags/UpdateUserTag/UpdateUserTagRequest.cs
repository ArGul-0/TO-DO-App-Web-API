using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.UseCases.Tags.UpdateUserTag
{
    public record UpdateUserTagRequest(
        [Required, StringLength(50)] string Name
        );
}
