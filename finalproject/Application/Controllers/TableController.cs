using Core.Features.Queries.GetTodos;
using MediatR;
using Persistence.DatabaseContext;
using Persistence.Models;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;
using Core.Features.Commands.CreateTodo;
using Core.Features.Commands.UpdateTodo;
using Core.Features.Commands.DeleteTodo;
using Microsoft.EntityFrameworkCore;

namespace Application.Controllers;

public class TableController : BaseController
{
    private readonly IMediator _mediator;
    private readonly TableContext _context;
    private TableContext? context;

    public TableController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("v1/todo/{id}")]
    public async Task<GetTodoResponse> GetTodos(Guid id)
    {
        var request = new GetTodoQuery()
        {
            TodoId = id
        };
        var response = await _mediator.Send(request);
        return response;
    }


    [HttpPost("v1/todo")]
    public async Task<IActionResult> CreateTodo([FromBody] CreateTodoCommand command)
    {
        if (command == null)
        {
            return BadRequest("Invalid table data.");
        }

        var response = await _mediator.Send(command);

        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response.Message);
    }

    [HttpPut("v1/todo/{id}")]
    public async Task<IActionResult> UpdateTodo(Guid id, [FromBody] UpdateTodoCommand command)
    {
        if (command == null || id != command.TodoId)
        {
            return BadRequest("Invalid table data.");
        }

        var response = await _mediator.Send(command);

        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response.Message);
    }

    [HttpGet("Get all - Pagination")]
    public async Task<IActionResult> GetAllTodos(int pageNumber = 1, int pageSize = 10)
    {
        var todos = await _context.Todos
            .Include(t => t.TodoDetails)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new
            {
                t.Day,
                t.TodayDate,
                t.Note,
                DetailCount = t.TodoDetails.Count // Menghitung jumlah TodoDetail terkait
            })
            .ToListAsync();

        return Ok(todos);
    }

    [HttpGet("v1/tododetail/{todoId}")]
    public async Task<IActionResult> GetTodoDetails(Guid todoId)
    {
        var todoDetails = await _context.TodoDetails
            .Where(td => td.TodoId == todoId)
            .Select(td => new
            {
                td.Activity,
                td.Category,
                td.DetailNote
            })
            .ToListAsync();

        return Ok(todoDetails);
    }


    [HttpDelete("v1/todo/{id}")]
    public async Task<IActionResult> DeleteTodo(Guid id)
    {
        var command = new DeleteTodoCommand
        {
            TodoId = id
        };

        var response = await _mediator.Send(command);

        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response.Message);
    }
}