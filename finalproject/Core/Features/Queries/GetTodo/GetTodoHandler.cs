using MediatR;
using Persistence.Repositories;
using StackExchange.Redis;
using System.Text.Json;

namespace Core.Features.Queries.GetTodos;

public class GetTodoHandler : IRequestHandler<GetTodoQuery, GetTodoResponse>
{
    private readonly ITodoRepository _todoRepository;
    private readonly IConnectionMultiplexer _redis;

    public GetTodoHandler(ITodoRepository todoRepository, IConnectionMultiplexer redis)
    {
        _todoRepository = todoRepository;
        _redis = redis;
    }

    public async Task<GetTodoResponse> Handle(GetTodoQuery query, CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();
        string cacheKey = $"Todo_{query.TodoId}";

        try
        {
            // Coba ambil data dari Redis
            string cachedData = await db.StringGetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<GetTodoResponse>(cachedData);
            }
        }
        catch (RedisConnectionException)
        {
            // Log Redis error 
            Console.WriteLine("Redis tidak tersedia, melanjutkan ke SQL");
        }

        // Jika data tidak ada di Redis atau Redis gagal, ambil dari SQL
        var todo = await _todoRepository.GetByIdAsync(query.TodoId);

        if (todo is null)
            return new GetTodoResponse();

        var response = new GetTodoResponse
        {
            TodoId = todo.TodoId,
            TodayDate = todo.TodayDate,
            Day = todo.Day,
            Note = todo.Note,
            DetailCount = todo.DetailCount
        };

        // Simpan data ke Redis (jika Redis tersedia kembali)
        try
        {
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(response), TimeSpan.FromMinutes(10));
        }
        catch (RedisConnectionException)
        {
            // Redis masih tidak tersedia, abaikan dan teruskan
            Console.WriteLine("Gagal menyimpan ke Redis, melanjutkan tanpa cache");
        }

        return response;
    }
}
