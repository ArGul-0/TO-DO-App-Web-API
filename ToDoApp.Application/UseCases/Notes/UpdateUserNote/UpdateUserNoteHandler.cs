using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.Security.Notes;
using ToDoApp.Application.UseCases.Notes.DeleteUserNote;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.UseCases.Notes.UpdateUserNote
{
    public class UpdateUserNoteHandler
    {
        private readonly INotesRepository notesRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly INotesAuthorizationService notesAuthorizationService;
        private readonly ILogger<UpdateUserNoteHandler> logger;
        public UpdateUserNoteHandler(
            INotesRepository notesRepository,
            IUnitOfWork unitOfWork,
            INotesAuthorizationService notesAuthorizationService,
            ILogger<UpdateUserNoteHandler> logger)
        {
            this.notesRepository = notesRepository;
            this.unitOfWork = unitOfWork;
            this.notesAuthorizationService = notesAuthorizationService;
            this.logger = logger;
        }

        public async Task<Result> Handle(UpdateUserNoteRequest request, int userId, int noteId)
        {
            var note = await notesRepository.GetNoteByIdWithTracking(noteId);

            if (note is null)
                return ResultT<NoteDto>.Failure(NotesErrors.NoteNotFound);

            bool result = notesAuthorizationService.IsUserOwnsNote(userId, note);

            if (!result)
                return ResultT<NoteDto>.Failure(NotesErrors.Forbidden);

            note.UpdateTitle(request.Title);
            note.UpdateContent(request.Content);
            note.UpdateIsDone(request.IsDone);

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User with id {userId} successfully update note with id {noteId}", userId, noteId );

            return Result.Success();
        }
    }
}
