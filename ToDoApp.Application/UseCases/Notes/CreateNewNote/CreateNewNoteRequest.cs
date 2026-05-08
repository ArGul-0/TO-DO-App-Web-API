using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.UseCases.Notes.CreateNewNote
{
    internal record CreateNewNoteRequest(
        [Required]string Title,
        string Content
    );
}
