using MediatR;
using ApiGestionPersonal.Application.Common.DTOs;
using ApiGestionPersonal.Application.Common.DTOs.TaskDtos;
using ApiGestionPersonal.Application.Common.Interfaces;
using ApiGestionPersonal.Domain.Enums;

namespace ApiGestionPersonal.Application.Features.Tasks.Queries;

public class GetTasksQuery : IRequest<PaginatedResponse<TaskResponse>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public string? Prioridad { get; set; }
    public bool? Completada { get; set; }
}

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, PaginatedResponse<TaskResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTasksQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedResponse<TaskResponse>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var allTasks = (await _unitOfWork.Tasks.GetAllAsync()).ToList();
        var categories = (await _unitOfWork.Categories.GetAllAsync()).ToDictionary(c => c.Id, c => c.Nombre);

        // Apply filters
        var filteredTasks = allTasks.AsEnumerable();

        if (request.FechaDesde.HasValue)
            filteredTasks = filteredTasks.Where(t => t.FechaCreacion >= request.FechaDesde.Value);

        if (request.FechaHasta.HasValue)
            filteredTasks = filteredTasks.Where(t => t.FechaCreacion <= request.FechaHasta.Value);

        if (!string.IsNullOrEmpty(request.Prioridad) && Enum.TryParse<Prioridad>(request.Prioridad, true, out var prioridad))
            filteredTasks = filteredTasks.Where(t => t.Prioridad == prioridad);

        if (request.Completada.HasValue)
            filteredTasks = filteredTasks.Where(t => t.Completada == request.Completada.Value);

        var totalCount = filteredTasks.Count();

        // Apply pagination
        var pagedTasks = filteredTasks
            .OrderByDescending(t => t.Prioridad)
            .ThenByDescending(t => t.FechaCreacion)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(t => new TaskResponse
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Contenido = t.Contenido,
                FechaVencimiento = t.FechaVencimiento,
                Prioridad = t.Prioridad.ToString(),
                Completada = t.Completada,
                CategoriaId = t.CategoriaId,
                CategoriaNombre = categories.GetValueOrDefault(t.CategoriaId, "General"),
                FechaCreacion = t.FechaCreacion
            })
            .ToList();

        return new PaginatedResponse<TaskResponse>
        {
            Items = pagedTasks,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}