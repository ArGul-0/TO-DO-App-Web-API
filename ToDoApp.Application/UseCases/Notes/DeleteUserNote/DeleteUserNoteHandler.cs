using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.Security.Notes;

namespace ToDoApp.Application.UseCases.Notes.DeleteUserNote
{
    public class DeleteUserNoteHandler
    {
        private readonly INoteRepository notesRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly INotesAuthorizationService notesAuthorizationService;
        private readonly ILogger<DeleteUserNoteHandler> logger;
        public DeleteUserNoteHandler(
            INoteRepository notesRepository,
            IUnitOfWork unitOfWork,
            INotesAuthorizationService notesAuthorizationService,
            ILogger<DeleteUserNoteHandler> logger)
        {
            this.notesRepository = notesRepository;
            this.unitOfWork = unitOfWork;
            this.notesAuthorizationService = notesAuthorizationService;
            this.logger = logger;
        }

        public async Task<Result> Handle(int userId, int noteId)
        {
            var note = await notesRepository.GetNoteById(noteId);

            if (note is null)
                return Result.Failure(NotesErrors.NoteNotFound);

            bool result = notesAuthorizationService.IsUserOwnsNote(userId, note);

            if (!result)
                return Result.Failure(NotesErrors.Forbidden);

            await notesRepository.DeleteNoteAsync(noteId);

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User with id {userId} successfully delete note with id {noteId}", userId, noteId);

            return Result.Success();
        }
    }
}
