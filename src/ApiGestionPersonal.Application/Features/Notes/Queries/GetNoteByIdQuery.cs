using MediatR;
using ApiGestionPersonal.Application.Common.DTOs.NoteDtos;
using ApiGestionPersonal.Application.Common.Interfaces;

namespace ApiGestionPersonal.Application.Features.Notes.Queries;

public class GetNoteByIdQuery : IRequest<NoteResponse>
{
    public int Id { get; set; }
}

public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, NoteResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetNoteByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NoteResponse> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
    {
        var note = await _unitOfWork.Notes.GetByIdAsync(request.Id);
        if (note == null)
            throw new KeyNotFoundException($"Note with id {request.Id} not found");

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