using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users;

namespace ToDoApp.Application.UseCases.Notes.CreateNewNote
{
    public class CreateNewNoteHandler
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<CreateNewNoteHandler> logger;
        private readonly IUnitOfWork unitOfWork;

        public CreateNewNoteHandler(IUserRepository userRepository, ILogger<CreateNewNoteHandler> logger, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ResultT<NoteDto>> Handle(CreateNewNoteRequest request, int userId)
        {
            var user = await userRepository.GetUserWithNotesAsync(userId);

            if(user is null)
            {
                logger.LogWarning("User with id {UserId} not found when trying to create a new note", userId);

                return ResultT<NoteDto>.Failure(UsersErrors.UserNotFound);
            }

            user.AddNote(request.Title, request.Content, request.IsDone);

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("New note created for user with id {UserId}", userId);

            var noteDto = new NoteDto(
                Id: user.Notes.Last().Id,
                Title: request.Title,
                Content: request.Content,
                CreatedAt: user.Notes.Last().CreatedAt,
                UpdatedAt: user.Notes.Last().UpdatedAt,
                IsDone: request.IsDone
                );

            return ResultT<NoteDto>.Success(noteDto);
        }
    }
}
