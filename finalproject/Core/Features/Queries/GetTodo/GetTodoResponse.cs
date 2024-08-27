namespace Core.Features.Queries.GetTodos;

public class GetTodoResponse
{
    public Guid TodoId { get; set; }
    public string Day { get; set; }
    public DateTime TodayDate { get; set; }
    public string Note { get; set; }
    public int DetailCount { get; set; }
}