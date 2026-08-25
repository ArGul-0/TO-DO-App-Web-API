using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;

namespace ToDoApp.Application.UseCases.Tags.CreateNewTag
{
    public class CreateNewTagHandler
    {
        public CreateNewTagHandler()
        {
            
        }

        public Task<ResultT<TagDto>> Handle(CreateNewTagRequest request, int userId)
        {

        }
    }
}
