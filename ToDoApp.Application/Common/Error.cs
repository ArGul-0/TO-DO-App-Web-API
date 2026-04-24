namespace ToDoApp.Application.Common
{
    public record Error(string Code, string Description, ErrorType Type)
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
    }
}
