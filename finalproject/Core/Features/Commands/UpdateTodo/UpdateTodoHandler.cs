using MediatR;
using Persistence.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Commands.UpdateTodo
{
    public class UpdateTodoHandler : IRequestHandler<UpdateTodoCommand, UpdateTodoResponse>
    {
        private readonly ITodoRepository _todoRepository;

        public UpdateTodoHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<UpdateTodoResponse> Handle(UpdateTodoCommand command, CancellationToken cancellationToken)
        {
            var todo = await _todoRepository.GetByIdAsync(command.TodoId);

            if (todo == null)
            {
                return new UpdateTodoResponse
                {
                    Success = false,
                    Message = "not found."
                };
            }

            todo.Day = command.Day;
            todo.TodayDate = command.TodayDate;
            todo.Note = command.Note;
            todo.DetailCount = command.DetailCount;

            await _todoRepository.UpdateAsync(todo);

            return new UpdateTodoResponse
            {
                Success = true,
                Message = "updated successfully."
            };
        }
    }
}
