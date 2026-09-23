using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.Security.Notes;
using ToDoApp.Application.UseCases.Users;

namespace ToDoApp.Application.UseCases.Notes.DetachTagFromNote
{
    public class DetachTagFromNoteHandler
    {
        private readonly IUserRepository userRepository;
        private readonly INoteRepository noteRepository;
        private readonly INotesAuthorizationService notesAuthorizationService;
        private readonly ILogger<DetachTagFromNoteHandler> logger;
        private readonly IUnitOfWork unitOfWork;

        public DetachTagFromNoteHandler(IUserRepository userRepository,
            INoteRepository noteRepository,
            INotesAuthorizationService notesAuthorizationService,
            ILogger<DetachTagFromNoteHandler> logger,
            IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.noteRepository = noteRepository;
            this.notesAuthorizationService = notesAuthorizationService;
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(int noteId, int tagId, int userId)
        {
            var existingUser = await userRepository.GetUserByIdAsync(userId);

            if (existingUser is null)
            {
                logger.LogWarning("Authenticated user with ID {UserId} was not found in the database while detaching a tag from a note.", userId);

                return Result.Failure(UsersErrors.UserNotFound);
            }

            var existingNote = await noteRepository.GetNoteByIdWithTrackingIncludeNoteTagsAsync(noteId);

            if (existingNote is null)
                return Result.Failure(NotesErrors.NoteNotFound);

            if (!notesAuthorizationService.IsUserOwnsNote(userId, existingNote))
                return Result.Failure(NotesErrors.Forbidden);

            var existingNoteTag = existingNote.NoteTags.FirstOrDefault(nt => nt.TagId == tagId);

            if (existingNoteTag is null)
                return Result.Failure(DetachTagFromNoteErrors.TagNotAttachedToNote);

            existingNote.RemoveTag(existingNoteTag);

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User with id {UserId} detached tag with id {TagId} from note with id {NoteId}", userId, tagId, noteId);

            return Result.Success();
        }
    }
}
