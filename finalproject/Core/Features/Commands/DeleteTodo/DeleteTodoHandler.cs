using MediatR;
using Persistence.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Commands.DeleteTodo
{
    public class DeleteTodoHandler : IRequestHandler<DeleteTodoCommand, DeleteTodoResponse>
    {
        private readonly ITodoRepository _todoRepository;

        public DeleteTodoHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<DeleteTodoResponse> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
        {
            var todo = await _todoRepository.GetByIdAsync(command.TodoId);

            if (todo == null)
            {
                return new DeleteTodoResponse
                {
                    Success = false,
                    Message = "not found."
                };
            }

            await _todoRepository.DeleteAsync(todo);

            return new DeleteTodoResponse
            {
                Success = true,
                Message = "deleted successfully."
            };
        }
    }
}
