using Microsoft.EntityFrameworkCore;
using Persistence.DatabaseContext;
using Persistence.Models;

namespace Persistence.Repositories;

public class TodoRepository : GenericRepository<Todo>, ITodoRepository
{
    private readonly TableContext _context;
    public TodoRepository(TableContext context) : base(context)
    {
        _context = context;

    }
    public async Task AddAsync(Todo todo)
    {
         _context.Todos.Add(todo);
        await _context.SaveChangesAsync();
    }
    public async Task<Todo> GetByIdAsync(Guid todoId)
    {
        return await _context.Todos.FindAsync(todoId);
    }

    public async Task UpdateAsync(Todo todo)
    {
        _context.Todos.Update(todo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Todo todo)
    {
        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
    }
}