using ToDoApp.Application.DTOs;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Common.Mappings
{
    public static class NoteMappings
    {
        extension(Note note)
        {
            public NoteDto ToDto()
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
}
