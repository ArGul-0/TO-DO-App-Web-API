using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users;

namespace ToDoApp.Application.UseCases.Notes.GetUserNotesByTag
{
    public class GetUserNotesByTagHandler
    {
        private readonly INoteRepository noteRepository;
        private readonly IUserRepository userRepository;
        private readonly ILogger<GetUserNotesByTagHandler> logger;

        public GetUserNotesByTagHandler(INoteRepository noteRepository,
            IUserRepository userRepository,
            ILogger<GetUserNotesByTagHandler> logger)
        {
            this.noteRepository = noteRepository;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<ResultT<List<NoteDto>>> Handle(int tagId, int userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);

            if (user is null)
            {
                logger.LogWarning("User with id {UserId} not found when trying to retrieve notes by tag", userId);

                return ResultT<List<NoteDto>>.Failure(UsersErrors.UserNotFound);
            }

            var notes = await noteRepository.GetAllNotesByTagAndUserIdAsync(tagId, userId);

            var noteDtos = notes.Select(n => n.ToDto()).ToList();

            return ResultT<List<NoteDto>>.Success(noteDtos);
        }
    }
}
