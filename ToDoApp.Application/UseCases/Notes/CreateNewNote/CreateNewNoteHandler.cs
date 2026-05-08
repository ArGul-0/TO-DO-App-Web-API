using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users;

namespace ToDoApp.Application.UseCases.Notes.CreateNewNote
{
    public class CreateNewNoteHandler
    {
        private readonly INotesRepository notesRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateNewNoteHandler(INotesRepository notesRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.notesRepository = notesRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ResultT<NoteDto>> Handle(CreateNewNoteRequest request, int userId)
        {
            var user = await userRepository.GetUserWithNotesAsync(userId);

            if(user is null)
                return ResultT<NoteDto>.Failure(UsersErrors.UserNotFound);

            user.AddNote(request.Title, request.Content);

            await unitOfWork.SaveChangesAsync();

            var noteDto = new NoteDto(
                Id: user.Notes.Last().Id,
                Title: request.Title,
                Content: request.Content
                );

            return ResultT<NoteDto>.Success(noteDto);
        }
    }
}
