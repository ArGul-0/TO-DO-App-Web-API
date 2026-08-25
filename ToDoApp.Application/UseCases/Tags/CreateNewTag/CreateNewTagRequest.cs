using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.UseCases.Tags.CreateNewTag
{
    public record CreateNewTagRequest(
        [Required] string Name
        );
}
