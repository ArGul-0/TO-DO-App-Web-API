namespace ToDoApp.Application.DTOs
{
    public record NoteDto(
        int Id,
        string Title,
        string Content
    );
}
