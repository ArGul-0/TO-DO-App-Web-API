using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;

namespace ToDoApp.Application.UseCases.Tags.GetAllMyTags
{
    public class GetAllMyTagsHandler
    {
        public GetAllMyTagsHandler() 
        {

        }



        public async Task<ResultT<List<TagDto>>> Handle(int userId)
        {
            var tagDtos = new List<TagDto>();
            return ResultT<List<TagDto>>.Success(tagDtos);
        }
    }
}
