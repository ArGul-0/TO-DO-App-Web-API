using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.Security.Notes;

namespace ToDoApp.Application.UseCases.Notes.DeleteUserNote
{
    public class DeleteUserNoteHandler
    {
        private readonly IUserRepository userRepository;
        private readonly INotesRepository notesRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly INotesAuthorizationService notesAuthorizationService
        public DeleteUserNoteHandler(
            IUserRepository userRepository,
            INotesRepository notesRepository,
            IUnitOfWork unitOfWork,
            INotesAuthorizationService notesAuthorizationService)
        {
            this.userRepository = userRepository;
            this.notesRepository = notesRepository;
            this.unitOfWork = unitOfWork;
            this.notesAuthorizationService = notesAuthorizationService;
        }

        public Task<Result> Handle(int userId)
        {

        }
    }
}
