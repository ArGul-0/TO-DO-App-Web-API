using ToDoApp.Application.DTOs;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Common.Mappings
{
    public static class TagMappings
    {
        public static TagDto ToDto(this Tag tag)
        {
            return new TagDto(
                Name: tag.Name
            );
        }
    }
}