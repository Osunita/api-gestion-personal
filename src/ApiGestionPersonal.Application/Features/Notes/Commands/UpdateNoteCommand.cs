using MediatR;
using ApiGestionPersonal.Application.Common.DTOs.NoteDtos;
using ApiGestionPersonal.Application.Common.Interfaces;

namespace ApiGestionPersonal.Application.Features.Notes.Commands;

public class UpdateNoteCommand : IRequest<NoteResponse>
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Contenido { get; set; }
    public string? Color { get; set; }
    public int? CategoriaId { get; set; }
}

public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, NoteResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateNoteCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NoteResponse> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await _unitOfWork.Notes.GetByIdAsync(request.Id);
        if (note == null)
            throw new KeyNotFoundException($"Note with id {request.Id} not found");

        note.Titulo = request.Titulo;
        note.Contenido = request.Contenido;
        note.Color = request.Color;
        note.CategoriaId = request.CategoriaId ?? note.CategoriaId;

        await _unitOfWork.Notes.UpdateAsync(note);
        await _unitOfWork.SaveChangesAsync();

        var category = await _unitOfWork.Categories.GetByIdAsync(note.CategoriaId);

        return new NoteResponse
        {
            Id = note.Id,
            Titulo = note.Titulo,
            Contenido = note.Contenido,
            Color = note.Color,
            CategoriaId = note.CategoriaId,
            CategoriaNombre = category?.Nombre ?? "General",
            FechaCreacion = note.FechaCreacion
        };
    }
}