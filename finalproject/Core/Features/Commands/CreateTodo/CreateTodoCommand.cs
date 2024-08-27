using MediatR;

namespace Core.Features.Commands.CreateTodo
{
    public class CreateTodoCommand : IRequest<CreateTodoResponse>
    {
        public string Day { get; set; }
        public DateTime TodayDate { get; set; }
        public string Note { get; set; }
        public int DetailCount { get; set; }
    }
}
