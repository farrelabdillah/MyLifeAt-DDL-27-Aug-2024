using Persistence.Models;
using System.Threading.Tasks;
namespace Persistence.Repositories;

public interface ITodoRepository : IGenericRepository<Todo>
{
    Task AddAsync(Todo todo);
    Task<Todo> GetByIdAsync(Guid todoId);
    Task UpdateAsync(Todo todo);
    Task DeleteAsync(Todo todo);
}