using Persistence.Models;

namespace Persistence.Repositories;

public interface IGenericRepository<T> where T : class
{
    List<T> GetAll();
    T? GetById(Guid id);
    Task<Todo> GetByIdAsync(Guid todoId);
    void Remove(T itemToRemove);
}