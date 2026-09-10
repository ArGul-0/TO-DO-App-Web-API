using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users;

namespace ToDoApp.Application.UseCases.Notes.AttachTagToNote
{
    public class AttachTagToNoteHandler
    {
        private readonly IUserRepository userRepository;
        private readonly INoteRepository noteRepository;
        private readonly ITagRepository tagRepository;
        private readonly ILogger<AttachTagToNoteHandler> logger;
        private readonly IUnitOfWork unitOfWork;

        public AttachTagToNoteHandler(IUserRepository userRepository,
            INoteRepository noteRepository,
            ITagRepository tagRepository,
            ILogger<AttachTagToNoteHandler> logger,
            IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.tagRepository = tagRepository;
            this.noteRepository = noteRepository;
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

            return Result.Success();
        }
    }
}
