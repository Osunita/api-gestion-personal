using MediatR;
using ApiGestionPersonal.Application.Common.DTOs;
using ApiGestionPersonal.Application.Common.DTOs.NoteDtos;
using ApiGestionPersonal.Application.Common.Interfaces;

namespace ApiGestionPersonal.Application.Features.Notes.Queries;

public class GetNotesQuery : IRequest<PaginatedResponse<NoteResponse>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int? CategoriaId { get; set; }
}

public class GetNotesQueryHandler : IRequestHandler<GetNotesQuery, PaginatedResponse<NoteResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetNotesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedResponse<NoteResponse>> Handle(GetNotesQuery request, CancellationToken cancellationToken)
    {
        var allNotes = (await _unitOfWork.Notes.GetAllAsync()).ToList();
        var categories = (await _unitOfWork.Categories.GetAllAsync()).ToDictionary(c => c.Id, c => c.Nombre);

        var filteredNotes = allNotes.AsEnumerable();

        if (request.CategoriaId.HasValue)
            filteredNotes = filteredNotes.Where(n => n.CategoriaId == request.CategoriaId.Value);

        var totalCount = filteredNotes.Count();

        var pagedNotes = filteredNotes
            .OrderByDescending(n => n.FechaCreacion)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(n => new NoteResponse
            {
                Id = n.Id,
                Titulo = n.Titulo,
                Contenido = n.Contenido,
                Color = n.Color,
                CategoriaId = n.CategoriaId,
                CategoriaNombre = categories.GetValueOrDefault(n.CategoriaId, "General"),
                FechaCreacion = n.FechaCreacion
            })
            .ToList();

        return new PaginatedResponse<NoteResponse>
        {
            Items = pagedNotes,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}