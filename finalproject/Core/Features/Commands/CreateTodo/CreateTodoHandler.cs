using MediatR;
using Persistence.Models;
using Persistence.Repositories;
using StackExchange.Redis;
using System.Text.Json;

namespace Core.Features.Commands.CreateTodo
{
    public class CreateTodoHandler : IRequestHandler<CreateTodoCommand, CreateTodoResponse>
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IConnectionMultiplexer _redis;

        public CreateTodoHandler(ITodoRepository todoRepository, IConnectionMultiplexer redis)
        {
            _todoRepository = todoRepository;
            _redis = redis;
        }

        public async Task<CreateTodoResponse> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
        {
            var newTodo = new Todo
            {
                TodoId = Guid.NewGuid(),
                Day = command.Day,
                TodayDate = command.TodayDate,
                Note = command.Note,
                DetailCount = command.DetailCount
            };

            await _todoRepository.AddAsync(newTodo);

            // Sinkronisasi data ke Redis
            var db = _redis.GetDatabase();
            string cacheKey = $"Todo_{newTodo.TodoId}";

            var cacheData = new CreateTodoResponse
            {
                TodoId = newTodo.TodoId,
                Success = true,
                Message = "created successfully."
            };

            // Simpan ke Redis dengan TTL 10 menit
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(cacheData), TimeSpan.FromMinutes(10));

            return cacheData;
        }
    }
}
