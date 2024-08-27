using MediatR;
using System;

namespace Core.Features.Commands.DeleteTodo
{
    public class DeleteTodoCommand : IRequest<DeleteTodoResponse>
    {
        public Guid TodoId { get; set; }
    }
}
