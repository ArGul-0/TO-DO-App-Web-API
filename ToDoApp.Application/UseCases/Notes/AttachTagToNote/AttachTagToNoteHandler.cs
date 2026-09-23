using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.Security.Notes;
using ToDoApp.Application.UseCases.Tags;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.UseCases.Notes.AttachTagToNote
{
    public class AttachTagToNoteHandler
    {
        private readonly IUserRepository userRepository;
        private readonly INoteRepository noteRepository;
        private readonly ITagRepository tagRepository;
        private readonly INotesAuthorizationService notesAuthorizationService;
        private readonly ILogger<AttachTagToNoteHandler> logger;
        private readonly IUnitOfWork unitOfWork;

        public AttachTagToNoteHandler(IUserRepository userRepository,
            INoteRepository noteRepository,
            ITagRepository tagRepository,
            INotesAuthorizationService notesAuthorizationService,
            ILogger<AttachTagToNoteHandler> logger,
            IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.noteRepository = noteRepository;
            this.tagRepository = tagRepository;
            this.notesAuthorizationService = notesAuthorizationService;
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(int noteId, int tagId, int userId)
        {
            var existingUser = await userRepository.GetUserByIdAsync(userId);

            if (existingUser is null)
            {
                logger.LogWarning("Authenticated user with ID {UserId} was not found in the database while attaching a tag to a note.", userId);

                return Result.Failure(UsersErrors.UserNotFound);
            }

            var existingNote = await noteRepository.GetNoteByIdWithTrackingIncludeNoteTagsAsync(noteId);

            if (existingNote is null)
                return Result.Failure(NotesErrors.NoteNotFound);

            if (!notesAuthorizationService.IsUserOwnsNote(userId, existingNote))
                return Result.Failure(NotesErrors.Forbidden);

            var existingTag = await tagRepository.GetTagByIdAsync(tagId);

            if (existingTag is null)
                return Result.Failure(TagsErrors.TagNotFound);

            if (existingTag.OwnerId != userId)
                return Result.Failure(TagsErrors.Forbidden);

            if (existingNote.NoteTags.Any(nt => nt.TagId == tagId))
                return Result.Failure(AttachTagToNoteErrors.TagAlreadyAttachedToNote);

            var newNoteTag = new NoteTag(noteId, tagId);

            existingNote.AddTag(newNoteTag);

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User with id {UserId} attached tag with id {TagId} to note with id {NoteId}", userId, tagId, noteId);

            return Result.Success();
        }
    }
}
