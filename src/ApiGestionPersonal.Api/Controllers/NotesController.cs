using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiGestionPersonal.Application.Features.Notes.Commands;
using ApiGestionPersonal.Application.Features.Notes.Queries;

namespace ApiGestionPersonal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotes(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? categoriaId = null)
    {
        var query = new GetNotesQuery
        {
            Page = page,
            PageSize = pageSize,
            CategoriaId = categoriaId
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNote(int id)
    {
        try
        {
            var result = await _mediator.Send(new GetNoteByIdQuery { Id = id });
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Note with id {id} not found" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new CreateNoteCommand
        {
            Titulo = request.Titulo,
            Contenido = request.Contenido,
            Color = request.Color
        };

        var result = await _mediator.Send(command);
        
        return CreatedAtAction(nameof(GetNote), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNote(int id, [FromBody] UpdateNoteRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var command = new UpdateNoteCommand
            {
                Id = id,
                Titulo = request.Titulo,
                Contenido = request.Contenido,
                Color = request.Color,
                CategoriaId = request.CategoriaId
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Note with id {id} not found" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNote(int id)
    {
        var result = await _mediator.Send(new DeleteNoteCommand { Id = id });
        
        if (!result)
            return NotFound(new { message = $"Note with id {id} not found" });

        return NoContent();
    }
}