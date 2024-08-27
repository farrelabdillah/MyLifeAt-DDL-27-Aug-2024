namespace Core.Features.Commands.CreateTodo
{
    public class CreateTodoResponse
    {
        public Guid TodoId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
