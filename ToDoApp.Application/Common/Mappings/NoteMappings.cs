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
                    IsDone: note.IsDone,
                    Tags: note.NoteTags.Select(noteTag => noteTag.Tag.ToDto()).ToList(),
                    CreatedAt: note.CreatedAt,
                    UpdatedAt: note.UpdatedAt
                    );
            }
        }
    }
}
