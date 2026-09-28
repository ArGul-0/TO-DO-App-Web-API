using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Tags;
using ToDoApp.Application.UseCases.Users;

namespace ToDoApp.Application.UseCases.Notes.GetUserNotesByTag
{
    public class GetUserNotesByTagHandler
    {
        private readonly INoteRepository noteRepository;
        private readonly IUserRepository userRepository;
        private readonly ITagRepository tagRepository;
        private readonly ILogger<GetUserNotesByTagHandler> logger;

        public GetUserNotesByTagHandler(INoteRepository noteRepository,
            IUserRepository userRepository,
            ITagRepository tagRepository,
            ILogger<GetUserNotesByTagHandler> logger)
        {
            this.noteRepository = noteRepository;
            this.userRepository = userRepository;
            this.tagRepository = tagRepository;
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

            var tag = await tagRepository.GetTagByIdAsync(tagId);

            if (tag is null)
                return ResultT<List<NoteDto>>.Failure(TagsErrors.TagNotFound);

            if(tag.OwnerId != userId)
                return ResultT<List<NoteDto>>.Failure(TagsErrors.Forbidden);

            var notes = await noteRepository.GetAllNotesByTagAndUserIdAsync(tagId, userId);

            var noteDtos = notes.Select(n => n.ToDto()).ToList();

            return ResultT<List<NoteDto>>.Success(noteDtos);
        }
    }
}
