using System.Diagnostics.CodeAnalysis;
using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositoryes;

namespace ToDoApp.Application.UseCases.Notes.GetNoteById
{
    public class GetNoteByIdHandler
    {
        private readonly INotesRepository notesRepository;

        public GetNoteByIdHandler(INotesRepository notesRepository)
        {
            this.notesRepository = notesRepository;
        }

        public async Task<ResultT<NoteDto>> Handle(int id, int userId)
        {
            var note = await notesRepository.GetNoteByIdAsync(id, userId);

            if(note is null)
                return ResultT<NoteDto>.Failure(NotesErrors.NoteNotFound);

            var noteDto = new NoteDto(
                Id: note.Id,
                Title: note.Title,
                Content: note.Content);

            return ResultT<NoteDto>.Success(noteDto);   
        }
    }
}
