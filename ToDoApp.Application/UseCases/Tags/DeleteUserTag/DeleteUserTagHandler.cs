using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Application.UseCases.Tags.DeleteUserTag
{
    public class DeleteUserTagHandler
    {
        private readonly IUserRepository userRepository;
        private readonly ITagRepository tagRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<DeleteUserTagHandler> logger;

        public DeleteUserTagHandler(IUserRepository userRepository,
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork,
            ILogger<DeleteUserTagHandler> logger)
        {
            this.userRepository = userRepository;
            this.tagRepository = tagRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Result> Handle(int tagId, int userId)
        {

        }
    }
}
