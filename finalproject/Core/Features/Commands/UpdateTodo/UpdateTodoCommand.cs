using MediatR;

namespace Core.Features.Commands.UpdateTodo
{
    public class UpdateTodoCommand : IRequest<UpdateTodoResponse>
    {
        public Guid TodoId { get; set; }
        public string Day { get; set; }
        public DateTime TodayDate { get; set; }
        public string Note { get; set; }
        public int DetailCount { get; set; }
    }
}
