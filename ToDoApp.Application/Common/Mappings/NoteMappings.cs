using ToDoApp.Application.DTOs;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Common.Mappings
{
    public static class NoteMappings
    {
        public static NoteDto ToDto(this Note note)
        {
            return new NoteDto(
                Id: note.Id,
                Title: note.Title,
                Content: note.Content,
                CreatedAt: note.CreatedAt,
                UpdatedAt: note.UpdatedAt,
                IsDone: note.IsDone
                );
        }
    }
}
