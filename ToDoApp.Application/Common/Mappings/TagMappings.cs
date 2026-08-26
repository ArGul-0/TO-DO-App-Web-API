using ToDoApp.Application.DTOs;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Common.Mappings
{
    public static class TagMappings
    {
        extension(Tag tag)
        {
            public TagDto ToDto()
            {
                return new TagDto(
                    Id: tag.Id,
                    Name: tag.Name
                );
            }
        }
    }
}