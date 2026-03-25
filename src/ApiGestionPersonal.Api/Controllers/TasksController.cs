using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiGestionPersonal.Application.Features.Tasks.Commands;
using ApiGestionPersonal.Application.Features.Tasks.Queries;

namespace ApiGestionPersonal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]  // JWT required for all endpoints
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] DateTime? fechaDesde = null,
        [FromQuery] DateTime? fechaHasta = null,
        [FromQuery] string? prioridad = null,
        [FromQuery] bool? completada = null)
    {
        var query = new GetTasksQuery
        {
            Page = page,
            PageSize = pageSize,
            FechaDesde = fechaDesde,
            FechaHasta = fechaHasta,
            Prioridad = prioridad,
            Completada = completada
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        try
        {
            var result = await _mediator.Send(new GetTaskByIdQuery { Id = id });
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Task with id {id} not found" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new CreateTaskCommand
        {
            Titulo = request.Titulo,
            Contenido = request.Contenido,
            FechaVencimiento = request.FechaVencimiento,
            Prioridad = request.Prioridad
        };

        var result = await _mediator.Send(command);
        
        return CreatedAtAction(nameof(GetTask), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var command = new UpdateTaskCommand
            {
                Id = id,
                Titulo = request.Titulo,
                Contenido = request.Contenido,
                FechaVencimiento = request.FechaVencimiento,
                Prioridad = request.Prioridad,
                Completada = request.Completada,
                CategoriaId = request.CategoriaId
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Task with id {id} not found" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var result = await _mediator.Send(new DeleteTaskCommand { Id = id });
        
        if (!result)
            return NotFound(new { message = $"Task with id {id} not found" });

        return NoContent();
    }
}