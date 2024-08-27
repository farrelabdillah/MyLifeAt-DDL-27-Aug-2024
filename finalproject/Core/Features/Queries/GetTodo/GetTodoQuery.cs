using MediatR;

namespace Core.Features.Queries.GetTodos;

public class GetTodoQuery : IRequest<GetTodoResponse>
{
    public Guid TodoId { get; set; }
}