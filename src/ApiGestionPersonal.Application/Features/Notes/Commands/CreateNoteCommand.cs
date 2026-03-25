using MediatR;
using ApiGestionPersonal.Application.Common.DTOs.NoteDtos;
using ApiGestionPersonal.Application.Common.Interfaces;
using ApiGestionPersonal.Domain.Entities;

namespace ApiGestionPersonal.Application.Features.Notes.Commands;

public class CreateNoteCommand : IRequest<NoteResponse>
{
    public string Titulo { get; set; } = string.Empty;
    public string? Contenido { get; set; }
    public string? Color { get; set; }
}

public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, NoteResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategorizationService _categorizationService;

    public CreateNoteCommandHandler(IUnitOfWork unitOfWork, ICategorizationService categorizationService)
    {
        _unitOfWork = unitOfWork;
        _categorizationService = categorizationService;
    }

    public async Task<NoteResponse> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        var categoryName = _categorizationService.Categorize(request.Contenido ?? request.Titulo);
        var category = await _unitOfWork.CategoryRepository.GetByNameAsync(categoryName);
        
        if (category == null)
        {
            category = await _unitOfWork.CategoryRepository.GetByIdAsync(1);
        }

        var note = new Note
        {
            Titulo = request.Titulo,
            Contenido = request.Contenido,
            Color = request.Color,
            CategoriaId = category!.Id,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Notes.AddAsync(note);
        await _unitOfWork.SaveChangesAsync();

        return new NoteResponse
        {
            Id = note.Id,
            Titulo = note.Titulo,
            Contenido = note.Contenido,
            Color = note.Color,
            CategoriaId = note.CategoriaId,
            CategoriaNombre = category.Nombre,
            FechaCreacion = note.FechaCreacion
        };
    }
}