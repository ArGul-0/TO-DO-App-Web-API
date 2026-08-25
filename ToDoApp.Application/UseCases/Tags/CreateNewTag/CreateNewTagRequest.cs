using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.UseCases.Tags.CreateNewTag
{
    public record CreateNewTagRequest(
        [Required, StringLength(50)] string Name
        );
}
